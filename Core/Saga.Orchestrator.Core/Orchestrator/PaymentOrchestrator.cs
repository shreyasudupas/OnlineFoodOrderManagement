using MenuManagment.Mongo.Domain.Constants;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuMangement.HttpClient.Domain.Dtos;
using MenuMangement.HttpClient.Domain.Interfaces.Services;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
using MenuMangement.HttpClient.Domain.Models;
using MenuMangement.HttpClient.Domain.Orchestrator;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Saga.Orchestrator.Core.Factory.Payment;

namespace Saga.Orchestrator.Core.Orchestrator
{
    public class PaymentOrchestrator : IPaymentOrchestrator
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentOrchestrator> _logger;
        private readonly IIdsHttpClientWrapper _idsHttpClientWrapper;
        private readonly ICartInformationWrapper _cartInformationWrapper;
        private readonly IOrderService _orderService;
        private readonly IVendorSerivce _vendorSerivce;
        private readonly ISignalROrderClientWrapper _signalROrderClientWrapper;
        private readonly IPaymentFactory _paymentFactory;
        public PaymentOrchestrator(IPaymentService paymentService,
            ILogger<PaymentOrchestrator> logger,
            ICartInformationWrapper cartInformationWrapper,
            IIdsHttpClientWrapper idsHttpClientWrapper,
            IOrderService orderService,
            IVendorSerivce vendorSerivce,
            ISignalROrderClientWrapper signalROrderClientWrapper,
            IPaymentFactory paymentFactory)
        {
            _paymentService = paymentService;
            _logger = logger;
            _cartInformationWrapper = cartInformationWrapper;
            _idsHttpClientWrapper = idsHttpClientWrapper;
            _orderService = orderService;
            _vendorSerivce = vendorSerivce;
            _signalROrderClientWrapper = signalROrderClientWrapper;
            _paymentFactory = paymentFactory;
        }

        public async Task<PaymentProcessDto> PaymentProcess(PaymentInformationRecord paymentInformation)
        {
            PaymentProcessDto response = new PaymentProcessDto();
            try
            {
                if (paymentInformation is not null)
                {
                    var tokenResult = await _idsHttpClientWrapper.GetApiAccessToken();

                    if (!string.IsNullOrEmpty(tokenResult))
                    {
                        var token = JsonConvert.DeserializeObject<AccessTokenModel>(tokenResult);
                        var accessToken = token.AccessToken;

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            var paymentResult = await _paymentFactory.SpendUserPayment(paymentInformation.UserId, paymentInformation.PaymentInfo.SelectedPayment,
                                paymentInformation.PaymentInfo.TotalPrice);

                            if (paymentResult == CommonConstrants.EventSpentFailureResponse)
                            {
                                response.Message = "Payment Failed";
                                return response;
                            }
                            else
                            {
                                var vendorId = paymentInformation.CartInfo.MenuItems.Select(m => m.VendorId).First();
                                var vendorDetailFromApi = await _vendorSerivce.GetVendorById(vendorId, accessToken);

                                if (vendorDetailFromApi is null)
                                {
                                    return await CallReversePaymentAPI(paymentInformation, accessToken, response);
                                }
                                else
                                {
                                    OrderInformationDto orderInfo = BuildOrderPlacedInformationModel(paymentInformation, vendorDetailFromApi);
                                    orderInfo.MenuItems = paymentInformation.CartInfo.MenuItems;
                                    var orderResult = await _orderService.AddOrderInformation(orderInfo, accessToken);

                                    if (orderResult is not null)
                                    {
                                        _logger.LogInformation("Order status success");
                                    }
                                    else
                                    {
                                        return await CallReversePaymentAPI(paymentInformation, accessToken, response);
                                    }

                                    //order info send to signalR service
                                    var orderPublishResult = _signalROrderClientWrapper.PostCallAsync(orderResult, accessToken);
                                    if (orderPublishResult is null)
                                    {
                                        _logger.LogError("Order Publishing to SignalR Service failed");
                                    }

                                    //clear the cart
                                    var cartResult = await _cartInformationWrapper.ClearCartInformation(paymentInformation.UserId, accessToken);
                                    if (cartResult)
                                    {
                                        _logger.LogInformation("Cart Clearing operation completed");
                                        response.Message = "Payment Operation completed";
                                    }
                                    else
                                    {
                                        _logger.LogError("Error in Cart Clearing operation");
                                        response.Message = "Error occured in clearing the cart";
                                    }

                                    response.IsSuccess = cartResult;

                                    return response;
                                }
                            }
                        }
                        else
                        {
                            string error = "Access Token is empty";
                            _logger.LogError(error);
                            response.Message = error;
                        }
                    }
                    else
                    {
                        string error = "Token is empty";
                        _logger.LogWarning(error);
                        response.Message = error;
                    }
                }
                else
                {
                    string error = "Error in payment information model";
                    _logger.LogError(error);
                    response.Message = error;
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.IsSuccess = false;
                response.Message = "Exception has occured, Please check the logs";
                return response;
            }

            return response;
        }

        private static OrderInformationDto BuildOrderPlacedInformationModel(PaymentInformationRecord paymentInformation,
            VendorDto? vendorDetailFromApi)
        {
            //next add order to orderManagment microservice
            return new OrderInformationDto
            {
                CartId = paymentInformation.CartInfo.CartId,
                Id = "",
                TotalPrice = paymentInformation.PaymentInfo.TotalPrice,
                PaymentDetail = new PaymentOrderDetailDto
                {
                    MethodOfDelivery = paymentInformation.PaymentInfo.MethodOfDelivery,
                    PaymentSuccess = true,
                    SelectedPayment = paymentInformation.PaymentInfo.SelectedPayment,
                    Price = paymentInformation.PaymentInfo.TotalPrice
                },
                Status = new OrderStatusDetailDto
                {
                    OrderPlaced = DateTime.Now.ToString()
                },
                UserDetail = new UserOrderDetailsDto
                {
                    FullAddress = paymentInformation.UserAddress.FullAddress,
                    Latitude = paymentInformation.UserAddress.Latitude,
                    Longitude = paymentInformation.UserAddress.Longitude,
                    UserId = paymentInformation.UserId,
                    Area = paymentInformation.UserAddress.Area,
                    City = paymentInformation.UserAddress.City,
                    PhoneNumber = paymentInformation.UserAddress.PhoneNumber,
                    EmailId = paymentInformation.UserAddress.EmailId
                },
                MenuItems = new List<MenuItemDto>(),
                VendorDetail = new VendorDetailDto
                {
                    VendorId = vendorDetailFromApi?.Id,
                    VendorName = vendorDetailFromApi?.VendorName
                }
            };
        }

        private async Task<PaymentProcessDto> CallReversePaymentAPI(PaymentInformationRecord paymentInformation,
            string token, PaymentProcessDto response)
        {
            var error = "Error occured in Order Microservice";
            _logger.LogError(error);

            var paymentResult = await _paymentFactory.AdjustUserPayment(paymentInformation,"SYSTEM_GENERATED");

            if (paymentResult == CommonConstrants.EventAdjustedFailureResponse)
            {
                _logger.LogError("Reverse Payment Failed");
            }

            //no clear cart
            response.Message = "Error occured during Payment";
            response.IsSuccess = false;

            return response;
        }
    }
}

using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.HttpClient.Domain.Dtos;
using MenuMangement.HttpClient.Domain.Interfaces.Services;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
using MenuMangement.HttpClient.Domain.Models;
using MenuMangement.HttpClient.Domain.Orchestrator;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        public PaymentOrchestrator(IPaymentService paymentService,
            ILogger<PaymentOrchestrator> logger,
            ICartInformationWrapper cartInformationWrapper,
            IIdsHttpClientWrapper idsHttpClientWrapper,
            IOrderService orderService,
            IVendorSerivce vendorSerivce,
            ISignalROrderClientWrapper signalROrderClientWrapper)
        {
            _paymentService = paymentService;
            _logger = logger;
            _cartInformationWrapper = cartInformationWrapper;
            _idsHttpClientWrapper = idsHttpClientWrapper;
            _orderService = orderService;
            _vendorSerivce = vendorSerivce;
            _signalROrderClientWrapper = signalROrderClientWrapper;
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

                        var currentPrice = paymentInformation.PaymentInfo.TotalPrice; //call API to get reward points
                        var accessToken = token.AccessToken;

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            var paymentSucess = await _paymentService.PaymentByRewardPoints(paymentInformation.UserId, accessToken, paymentInformation.PaymentInfo);

                            if (paymentSucess == false)
                            {
                                response.Message = "Payment Failed";
                                return response;
                            }
                            else
                            {
                                var vendorId = paymentInformation.CartInfo.MenuItems.Select(m => m.VendorId).First();
                                var vendorDetailFromApi = await _vendorSerivce.GetVendorById(vendorId, accessToken);

                                if(vendorDetailFromApi is null)
                                {
                                    return await CallReversePaymentAPI(paymentInformation, accessToken, response, currentPrice);
                                }
                                else
                                {
                                    //next add order to orderManagment microservice
                                    var orderInfo = new OrderInformationDto
                                    {
                                        CartId = paymentInformation.CartInfo.CartId,
                                        Id = "",
                                        OrderPlacedDateTime = "",
                                        TotalPrice = currentPrice,
                                        PaymentDetail = new PaymentOrderDetailDto
                                        {
                                            MethodOfDelivery = paymentInformation.PaymentInfo.MethodOfDelivery,
                                            PaymentSuccess = paymentSucess,
                                            SelectedPayment = paymentInformation.PaymentInfo.SelectedPayment,
                                            Price = paymentInformation.PaymentInfo.TotalPrice
                                        },
                                        OrderStatus = MenuManagment.Mongo.Domain.Enum.OrderStatusEnum.OrderPlaced,
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
                                            VendorId = vendorDetailFromApi.Id,
                                            VendorName = vendorDetailFromApi.VendorName
                                        }
                                    };
                                    orderInfo.MenuItems = paymentInformation.CartInfo.MenuItems;
                                    var orderResult = await _orderService.AddOrderInformation(orderInfo, accessToken);

                                    if (orderResult is not null)
                                    {
                                        _logger.LogInformation("Order status success");
                                    }
                                    else
                                    {
                                        return await CallReversePaymentAPI(paymentInformation, accessToken, response, currentPrice);
                                    }

                                    //order info send to signalR service
                                    var orderPublishResult = _signalROrderClientWrapper.PostCallAsync(orderResult, accessToken);
                                    if(orderPublishResult is null)
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
        

        private async Task<PaymentProcessDto> CallReversePaymentAPI(PaymentInformationRecord paymentInformation,
            string token, PaymentProcessDto response,
            double currentPrice)
        {
            var error = "Error occured in Order Microservice";
            _logger.LogError(error);

            var paymentRevertSucess = await _paymentService.PaymentByRewardPoints(paymentInformation.UserId, token,new PaymentDetailDto
            {
                TotalPrice = currentPrice
            });

            _logger.LogInformation($"Payment Update Revert Result: {paymentRevertSucess}");

            //no clear cart
            response.Message = "Error occured during Payment";
            response.IsSuccess = false;

            return response;
        }
    }
}

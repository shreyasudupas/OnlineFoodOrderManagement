using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.HttpClient.Domain.Dtos;
using MenuMangement.HttpClient.Domain.Interfaces.Services;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
using MenuMangement.HttpClient.Domain.Models;
using MenuMangement.HttpClient.Domain.Orchestrator;
using Microsoft.AspNetCore.SignalR;
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

                        var currentPrice = await _paymentFactory.GetPaymentBasedOnUserSelection(paymentInformation.UserId, paymentInformation.PaymentInfo); //call API to get reward points
                        var accessToken = token.AccessToken;

                        if(currentPrice is not null)
                        {
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                var body = new
                                {
                                    UserId = paymentInformation.UserId,
                                    AmountToBeDebited = paymentInformation.PaymentInfo.TotalPrice
                                };
                                var bodyContent = JsonConvert.SerializeObject(body);
                                var paymentSucess = await _paymentService.PaymentByRewardPoints("utility/update/points", paymentInformation.UserId, accessToken, bodyContent);

                                if (paymentSucess == false)
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
                                        return await CallReversePaymentAPI(paymentInformation, accessToken, response, currentPrice??0);
                                    }
                                    else
                                    {
                                        //next add order to orderManagment microservice
                                        var orderInfo = new OrderInformationDto
                                        {
                                            CartId = paymentInformation.CartInfo.CartId,
                                            Id = "",
                                            OrderPlacedDateTime = "",
                                            TotalPrice = currentPrice??0,
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
                                            return await CallReversePaymentAPI(paymentInformation, accessToken, response, currentPrice??0);
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
                            string error = "Error occured in Payment Selection Process";
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

            var body = new
            {
                UserId = paymentInformation.UserId,
                AmountToBeReversed = currentPrice
            };
            var bodyContent = JsonConvert.SerializeObject(body);
            var paymentRevertSucess = await _paymentService.PaymentByRewardPoints("utility/reverse/points", paymentInformation.UserId, token, bodyContent);

            _logger.LogInformation($"Payment Update Revert Result: {paymentRevertSucess}");

            //no clear cart
            response.Message = "Error occured during Payment";
            response.IsSuccess = false;

            return response;
        }
    }
}

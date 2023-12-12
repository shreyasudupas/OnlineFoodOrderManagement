﻿using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Saga.Orchestrator.Core.Interfaces.Orchestrator;
using Saga.Orchestrator.Core.Interfaces.Wrappers;
using Saga.Orchestrator.Core.Models;
using Saga.Orchestrator.Core.Models.Dtos;

namespace Saga.Orchestrator.Core.Orchestrator
{
    public class PaymentOrchestrator : IPaymentOrchestrator
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentOrchestrator> _logger;
        private readonly IIdsHttpClientWrapper _idsHttpClientWrapper;
        private readonly ICartInformationWrapper _cartInformationWrapper;
        public PaymentOrchestrator(IPaymentService paymentService,
            ILogger<PaymentOrchestrator> logger,
            ICartInformationWrapper cartInformationWrapper,
            IIdsHttpClientWrapper idsHttpClientWrapper)
        {
            _paymentService = paymentService;
            _logger = logger;
            _cartInformationWrapper = cartInformationWrapper;
            _idsHttpClientWrapper = idsHttpClientWrapper;
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

                        var currentPrice = paymentInformation.PaymentInfo.TotalPrice;
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
                                //next clear the cart

                                var cartResult = await _cartInformationWrapper.ClearCartInformation(paymentInformation.UserId, accessToken);
                                if(cartResult)
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

        private async Task<PaymentProcessDto> ReversePaymentProcess(PaymentInformationRecord paymentInformation,
            string token, PaymentProcessDto response,
            double currentPrice)
        {
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

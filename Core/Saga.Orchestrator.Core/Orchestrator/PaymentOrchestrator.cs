using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Saga.Orchestrator.Core.Interfaces;
using Saga.Orchestrator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Orchestrator.Core.Orchestrator
{
    public class PaymentOrchestrator
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentOrchestrator> _logger;
        private readonly IIdsHttpClientWrapper _idsHttpClientWrapper;
        public PaymentOrchestrator(IPaymentService paymentService,
            ILogger<PaymentOrchestrator> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<bool> PaymentOrchestrator(PaymentInformationRecord paymentInformation)
        {
            try
            {
                if (paymentInformation is not null)
                {
                    var tokenResult = await _idsHttpClientWrapper.GetApiAccessToken();

                    if (!string.IsNullOrEmpty(tokenResult))
                    {
                        var token = JsonConvert.DeserializeObject<AccessTokenModel>(tokenResult);

                        var currentPrice = paymentInformation.PayementDetails.Price;

                        if(!string.IsNullOrEmpty(token.AccessToken))
                        {
                            var paymentSucess = await _paymentService.PaymentByRewardPoints(paymentInformation.UserId, paymentInformation.PayementDetails);

                            if (paymentSucess == false)
                            {
                                var paymentRevertSucess = await _paymentService.PaymentByRewardPoints(paymentInformation.UserId, new PaymentDetailDto
                                {
                                    Price = currentPrice
                                });

                                _logger.LogInformation($"Payment Update Revert Result: {paymentRevertSucess}");

                                //no clear cart
                            }
                            else
                            {
                                //next clear the cart

                            }
                        }
                        else
                        {
                            _logger.LogError("Access Token is empty");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Token is empty");
                    }
                }
                else
                {
                    _logger.LogError("Error in payment information model");
                }
            } catch (Exception ex)
            {

            }
            
        }
    }
}

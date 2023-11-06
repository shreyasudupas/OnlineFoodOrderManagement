using Amazon.Runtime.Internal.Util;
using MenuManagement.HttpClient.Domain.Interface;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Core.Interfaces;
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
                if (paymentInformation != null)
                {
                    var currentPrice = paymentInformation.PayementDetails.Price;

                    var paymentSucess = await _paymentService.PaymentByRewardPoints(paymentInformation.UserId, paymentInformation.PayementDetails);

                    if (paymentSucess == false)
                    {
                        var paymentRevertSucess = await _paymentService.PaymentByRewardPoints(paymentInformation.UserId, new PaymentDetailDto
                        {
                            Price = currentPrice
                        });

                        _logger.LogInformation($"Payment Update Revert Result: {paymentRevertSucess}");
                    }
                    else
                    {
                        //next clear the cart

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

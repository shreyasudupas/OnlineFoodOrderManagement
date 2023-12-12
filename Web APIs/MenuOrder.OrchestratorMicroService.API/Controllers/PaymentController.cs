using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.Core.Commands.PaymentCommand;
using Saga.Orchestrator.Core.Models.Dtos;

namespace MenuOrder.OrchestratorMicroService.API.Controllers
{
    public class PaymentController : BaseController
    {
        [HttpPost("/api/payment")]
        public async Task<PaymentProcessDto> PaymentProcess([FromBody] PaymentInformationRecord paymentInfo)
        {
            return await Mediator.Send(new PaymentCommand
            {
                paymentInformation = paymentInfo
            });
        }
    }
}

using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using Saga.Orchestrator.Core.Models.Dtos;

namespace Saga.Orchestrator.Core.Interfaces.Orchestrator
{
    public interface IPaymentOrchestrator
    {
        Task<PaymentProcessDto> PaymentProcess(PaymentInformationRecord paymentInformation);
    }
}

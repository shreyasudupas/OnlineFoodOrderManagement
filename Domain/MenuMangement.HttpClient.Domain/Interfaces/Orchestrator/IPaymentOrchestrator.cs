using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.HttpClient.Domain.Dtos;

namespace MenuMangement.HttpClient.Domain.Orchestrator;

public interface IPaymentOrchestrator
{
    Task<PaymentProcessDto> PaymentProcess(PaymentInformationRecord paymentInformation);
}

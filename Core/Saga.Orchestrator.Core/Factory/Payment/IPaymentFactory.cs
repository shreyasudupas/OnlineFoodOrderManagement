using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace Saga.Orchestrator.Core.Factory.Payment
{
    public interface IPaymentFactory
    {
        Task<double?> GetPaymentBasedOnUserSelection(string userId, PaymentDetailDto paymentDetail);
    }
}
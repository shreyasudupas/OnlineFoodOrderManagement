using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace Saga.Orchestrator.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> PaymentByRewardPoints(string userId, PaymentDetailDto payementDetail);
    }
}

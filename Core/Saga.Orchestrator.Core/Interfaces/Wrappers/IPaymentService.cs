using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace Saga.Orchestrator.Core.Interfaces.Wrappers
{
    public interface IPaymentService
    {
        Task<bool> PaymentByRewardPoints(string userId, string token, PaymentDetailDto payementDetail);
    }
}

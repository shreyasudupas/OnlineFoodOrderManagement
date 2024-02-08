using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using Saga.Orchestrator.Core.Factory.Payment.Interfaces;

namespace Saga.Orchestrator.Core.Factory.Payment
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IPaymentByRewardManager _paymentByRewardManager;

        public PaymentFactory(IPaymentByRewardManager paymentByRewardManager)
        {
            _paymentByRewardManager = paymentByRewardManager;
        }

        public async Task<double?> GetPaymentBasedOnUserSelection(string userId, PaymentDetailDto paymentDetail)
        {
            if (paymentDetail.SelectedPayment == "Reward")
            {
                var paymentByReward = await _paymentByRewardManager.GetRewardFromUser(userId);
                return paymentByReward;
            }

            return null;
        }
    }
}

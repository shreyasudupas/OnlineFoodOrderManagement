using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.HttpClient.Domain.Exceptions;
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
                try
                {
                    var paymentByReward = await _paymentByRewardManager.GetRewardFromUser(userId);
                    return paymentByReward;

                } catch(GraphQLException ex)
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<string?> SpendUserPayment(string userId,string paymentMethod,double price)
        {
            if (paymentMethod == "Reward")
            {
                var result = await _paymentByRewardManager.SpendUserPoints(userId, price);
                return result;
            }

            return null;
        }

        public async Task<string?> AdjustUserPayment(PaymentInformationRecord paymentInformation,string adjustedUserId)
        {
            if (paymentInformation.PaymentInfo.SelectedPayment == "Reward")
            {
                var result = await _paymentByRewardManager.AdjustUserPoints(paymentInformation.UserId, paymentInformation.PaymentInfo.TotalPrice,
                    adjustedUserId);
                return result;
            }

            return null;
        }
    }
}

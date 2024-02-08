using MenuMangement.HttpClient.Domain.Interfaces.GraphQl;
using Saga.Orchestrator.Core.Factory.Payment.Interfaces;

namespace Saga.Orchestrator.Core.Factory.Payment.Managers
{
    public class PaymentByRewardManager : IPaymentByRewardManager
    {
        private readonly IGetUserRewardQuery _getUserRewardQuery;

        public PaymentByRewardManager(IGetUserRewardQuery getUserRewardQuery)
        {
            _getUserRewardQuery = getUserRewardQuery;
        }

        public async Task<double?> GetRewardFromUser(string userId)
        {
            var result = await _getUserRewardQuery.GetRewardFromUserAsync(userId);

            return result;
        }
    }
}

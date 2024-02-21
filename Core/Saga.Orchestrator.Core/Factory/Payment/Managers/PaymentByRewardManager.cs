using MenuMangement.HttpClient.Domain.Exceptions;
using MenuMangement.HttpClient.Domain.Interfaces.GraphQl;
using Saga.Orchestrator.Core.Factory.Payment.Interfaces;

namespace Saga.Orchestrator.Core.Factory.Payment.Managers
{
    public class PaymentByRewardManager : IPaymentByRewardManager
    {
        private readonly IUserPointsGraphQlClient _userPointsGraphQlClient;
        private readonly IUserPointsMuationGraphqlClient _spendUserPointsGraphqlClient;

        public PaymentByRewardManager(IUserPointsGraphQlClient userPointsGraphQlClient,
            IUserPointsMuationGraphqlClient spendUserPointsGraphqlClient)
        {
            _userPointsGraphQlClient = userPointsGraphQlClient;
            _spendUserPointsGraphqlClient = spendUserPointsGraphqlClient;
        }

        public async Task<double?> GetRewardFromUser(string userId)
        {
            var result = await _userPointsGraphQlClient.GetRewardPointsFromUserId(userId);

            return result;
        }

        public async Task<string?> SpendUserPoints(string userId,double points)
        {
            try
            {
                var userSpentEvent = await _spendUserPointsGraphqlClient.SpendUserPoints(userId, points);
                return userSpentEvent;
            } 
            catch(GraphQLException ex)
            {
                return null;
            }
        }

        public async Task<string?> AdjustUserPoints(string userId,double points,string adjustedUserId)
        {
            try
            {
                var userSpentEvent = await _spendUserPointsGraphqlClient.AdjustUserPoints(userId, points,adjustedUserId);
                return userSpentEvent;
            }
            catch (GraphQLException ex)
            {
                return null;
            }
        }
    }
}

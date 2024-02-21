using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Request;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.UserPointsCalculation.Aggregate;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class UserPointsMutationExtensionType
    {
        public async Task<UserPointResponse> AddPoints(UserPointsAddSpendInput userPointsAdd,
            [Service] UserPoint userPointService)
        {
            var result = await userPointService.AddPoints(userPointsAdd.UserId, userPointsAdd.Points);
            return result;
        }

        public async Task<UserPointResponse> SpendPoints(UserPointsAddSpendInput userPointsSpend,
            [Service] UserPoint userPointService)
        {
            var result = await userPointService.RemovePoints(userPointsSpend.UserId, userPointsSpend.Points);
            return result;
        }

        public async Task<UserPointResponse> UserPointsAdjusted(UserPointsAdjustedInput userPointsAdjusted,
            [Service] UserPoint userPointService)
        {
            var result = await userPointService.AdjustPoints(userPointsAdjusted.UserId
                ,userPointsAdjusted.Points
                ,userPointsAdjusted.AdjustedUserId);
            return result;
        }
    }
}

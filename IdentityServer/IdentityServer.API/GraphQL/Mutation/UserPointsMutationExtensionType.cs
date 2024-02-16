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

        public async Task<UserPointResponse> SpendPoints(UserPointsAddSpendInput userPointsAdd,
            [Service] UserPoint userPointService)
        {
            var result = await userPointService.RemovePoints(userPointsAdd.UserId, userPointsAdd.Points);
            return result;
        }

        public async Task<UserPointResponse> UserPointsAdjusted(UserPointsAdjustedInput userPointsAdjustedInput,
            [Service] UserPoint userPointService)
        {
            var result = await userPointService.AdjustPoints(userPointsAdjustedInput.UserId
                ,userPointsAdjustedInput.Points
                ,userPointsAdjustedInput.AdminId);
            return result;
        }
    }
}

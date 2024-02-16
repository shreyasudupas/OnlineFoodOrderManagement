using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Common.Interfaces.Repository;
using IdenitityServer.Core.Domain.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class UserPointsQueryExtensionType
    {
        public async Task<List<UserPointsEvent>> GetUserPointsEvents(string userId, 
            [Service] IUserPointEventRepository userPointEventRepository)
        {
            var results = await userPointEventRepository.GetAllUserEvents(userId);
            return results;
        }

        public async Task<UserPointsEvent> GetCurrentUserPointsEvent(string userId,
            [Service] IUserPointEventRepository userPointEventRepository)
        {
            var results = await userPointEventRepository.GetCurrentUserEvent(userId);
            return results;
        }
    }
}

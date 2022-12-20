using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.QueryResolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class UserInformationListExtensionType
    {
        public async Task<List<UserProfile>> UserList(
            [Service] GetUserListResolver getUserListResolver)
        {
            return await getUserListResolver.GetUserList();
        }
    }
}

using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.QueryResolvers;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class RoleByIdExtensionType
    {
        public RoleListResponse Role(string roleId,
        [Service] GetUserRolesResolver resolver)
        {
            return resolver.GetRoleById(roleId);
        }
    }
}

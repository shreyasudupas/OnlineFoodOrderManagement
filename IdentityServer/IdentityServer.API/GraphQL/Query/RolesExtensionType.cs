using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.QueryResolvers;
using System.Collections.Generic;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class RolesExtensionType
    {
        public List<RoleListResponse> Roles([Service] GetUserRolesResolver getUserRolesResolver)
        {
            return getUserRolesResolver.GetRoles();
        }

        public RoleListResponse Role(string roleId,
        [Service] GetUserRolesResolver resolver)
        {
            return resolver.GetRoleById(roleId);
        }
    }
}

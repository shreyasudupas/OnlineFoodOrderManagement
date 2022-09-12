using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.MutationResolver;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class SaveRoleExtensionType
    {
        public RoleListResponse SaveRole(string roleId, string roleName,
            [Service] MutationRoleResolver mutationRoleResolver)
        {
            return mutationRoleResolver.SaveRole(new RoleListResponse { RoleId = roleId , RoleName = roleName });
        }
    }
}

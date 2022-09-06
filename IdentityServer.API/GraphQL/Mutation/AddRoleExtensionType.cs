using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.MutationResolver;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class AddRoleExtensionType
    {
        public async Task<RoleListResponse> AddRole(RoleListResponse newRole,
            [Service] AddRoleResolver addRoleResolver)
        {
            return await addRoleResolver.AddRole(newRole);
        }
    }
}

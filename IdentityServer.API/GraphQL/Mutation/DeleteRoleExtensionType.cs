using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.MutationResolver;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class DeleteRoleExtensionType
    {
        public async Task<bool> DeleteRole(string roleId,
            [Service] MutationRoleResolver mutationRoleResolver)
        {
            return await mutationRoleResolver.DeleteRole(roleId);
        }
    }
}
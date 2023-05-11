using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.MutationResolver;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class IdentityResourceMutationExtensionType
    {
        public async Task<IdentityResourceModel> AddIdentityResource(IdentityResourceModel identityResourceModel,
            [Service] IdentityResourcesMutationResolver identityResourcesMutationResolver
            )
        {
            return await identityResourcesMutationResolver.AddIdentityResource(identityResourceModel);
        }

        public async Task<IdentityResourceModel> UpdateIdentityResource(IdentityResourceModel identityResourceModel,
            [Service] IdentityResourcesMutationResolver identityResourcesMutationResolver
            )
        {
            return await identityResourcesMutationResolver.UpdateIdentityResource(identityResourceModel);
        }

        public async Task<bool> DeleteIdentityResource(int id,
            [Service] IdentityResourcesMutationResolver identityResourcesMutationResolver
            )
        {
            return await identityResourcesMutationResolver.DeleteIdentityResource(id);
        }
    }
}

using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.MutationResolver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class ClientMutationExtensionType
    {
        public async Task<List<string>> SaveAllowedScopes(
            int clientId,List<string> scopes,
            [Service] ClientMutationResolver clientMutationResolver
            )
        {
            return await clientMutationResolver.SaveAllowedScope(clientId,scopes);
        }

        public async Task<List<string>> SaveAllowedGrantTypes(
            int clientId, List<string> selectedGrantTypes,
            [Service] ClientMutationResolver clientMutationResolver
            )
        {
            return await clientMutationResolver.SaveAllowedGrantTypes(clientId, selectedGrantTypes);
        }
    }
}

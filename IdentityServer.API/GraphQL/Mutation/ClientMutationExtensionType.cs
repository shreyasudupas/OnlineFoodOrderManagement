using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.MutationResolver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class ClientMutationExtensionType
    {
        public async Task<ClientBasicInfo> SaveClientBasicInformation(ClientBasicInfo clientBasicInfo,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.SaveClientBasicInformation(clientBasicInfo);
        }

        public async Task<List<string>> SaveAllowedScopes(
            int clientId,List<string> scopes,
            [Service] ClientMutationResolver clientMutationResolver
            )
        {
            return await clientMutationResolver.SaveAllowedScope(clientId,scopes);
        }

        public async Task<string> SaveAllowedGrantTypes(
            int clientId, string selectedGrantTypes,
            [Service] ClientMutationResolver clientMutationResolver
            )
        {
            return await clientMutationResolver.SaveAllowedGrantTypes(clientId, selectedGrantTypes);
        }
    }
}

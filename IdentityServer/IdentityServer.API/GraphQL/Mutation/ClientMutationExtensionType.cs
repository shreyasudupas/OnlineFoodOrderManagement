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

        public async Task<ClientSecretModel> SaveClientSecret(ClientSecretModel clientSecretModel,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.SaveClientSecret(clientSecretModel);
        }

        public async Task<ClientSecretModel> DeleteClientSecret(ClientSecretModel clientSecretModel,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.DeleteClientSecret(clientSecretModel);
        }

        public async Task<AllowedCrosOriginModel> SaveAllowedCorsOrigin(AllowedCrosOriginModel allowedCrosOriginModel,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.SaveClientAllowedCorsOrigin(allowedCrosOriginModel);
        }

        public async Task<AllowedCrosOriginModel> DeleteAllowedCorsOrigin(AllowedCrosOriginModel allowedCrosOriginModel,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.DeleteClientAllowedCorsOrigin(allowedCrosOriginModel);
        }

        public async Task<RedirectUrlModel> SaveClientRedirectUrl(RedirectUrlModel redirectUrlModel,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.SaveClientRedirectUrls(redirectUrlModel);
        }

        public async Task<RedirectUrlModel> DeleteClientRedirectUrl(RedirectUrlModel redirectUrlModel,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.DeleteClientRedirectUrls(redirectUrlModel);
        }

        public async Task<PostLogoutRedirectUriModel> SaveClientPostLogoutRedirectUrl(PostLogoutRedirectUriModel postLogoutRedirectUriModel,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.SaveClientPostLogoutRedirectUrls(postLogoutRedirectUriModel);
        }

        public async Task<PostLogoutRedirectUriModel> DeleteClientPostLogoutRedirectUrl(PostLogoutRedirectUriModel postLogoutRedirectUriModel,
            [Service] ClientMutationResolver clientMutationResolver)
        {
            return await clientMutationResolver.DeleteClientPostLogoutRedirectUrls(postLogoutRedirectUriModel);
        }
    }
}

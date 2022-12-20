using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class ClientMutationResolver
    {
        private readonly IClientService _clientService;

        public ClientMutationResolver(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<ClientBasicInfo> SaveClientBasicInformation(ClientBasicInfo clientModel)
        {
            return await _clientService.SaveClient(clientModel);
        }

        public async Task<List<string>> SaveAllowedScope(int clientId, List<string> scopes)
        {
            return await _clientService.SaveAllowedScopes(clientId, scopes);
        }

        public async Task<string> SaveAllowedGrantTypes(int clientId, string selectedGrantTypes)
        {
            return await _clientService.SaveAllowedGrants(clientId, selectedGrantTypes);
        }

        public async Task<ClientSecretModel> SaveClientSecret(ClientSecretModel clientSecretModel)
        {
            return await _clientService.SaveClientSecret(clientSecretModel);
        }

        public async Task<ClientSecretModel> DeleteClientSecret(ClientSecretModel clientSecretModel)
        {
            return await _clientService.DeleteClientSecret(clientSecretModel);
        }

        public async Task<AllowedCrosOriginModel> SaveClientAllowedCorsOrigin(AllowedCrosOriginModel allowedCrosOriginModel)
        {
            return await _clientService.SaveClientAllowedCorsOrigin(allowedCrosOriginModel);
        }

        public async Task<AllowedCrosOriginModel> DeleteClientAllowedCorsOrigin(AllowedCrosOriginModel allowedCrosOriginModel)
        {
            return await _clientService.DeleteClientAllowedCorsOrigin(allowedCrosOriginModel);
        }

        public async Task<RedirectUrlModel> SaveClientRedirectUrls(RedirectUrlModel redirectUrlModel)
        {
            return await _clientService.SaveClientRedirectUrls(redirectUrlModel);
        }

        public async Task<RedirectUrlModel> DeleteClientRedirectUrls(RedirectUrlModel redirectUrlModel)
        {
            return await _clientService.DeleteClientRedirectUrls(redirectUrlModel);
        }

        public async Task<PostLogoutRedirectUriModel> SaveClientPostLogoutRedirectUrls(PostLogoutRedirectUriModel postLogoutRedirectUriModel)
        {
            return await _clientService.SaveClientPostLogoutRedirectUrls(postLogoutRedirectUriModel);
        }

        public async Task<PostLogoutRedirectUriModel> DeleteClientPostLogoutRedirectUrls(PostLogoutRedirectUriModel postLogoutRedirectUriModel)
        {
            return await _clientService.DeleteClientPostLogoutRedirectUrls(postLogoutRedirectUriModel);
        }
    }
}

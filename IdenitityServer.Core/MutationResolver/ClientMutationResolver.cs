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
    }
}

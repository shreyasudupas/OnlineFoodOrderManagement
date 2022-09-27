using IdenitityServer.Core.Common.Interfaces;
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

        public async Task<List<string>> SaveAllowedScope(int clientId, List<string> scopes)
        {
            return await _clientService.SaveAllowedScopes(clientId, scopes);
        }

        public async Task<List<string>> SaveAllowedGrantTypes(int clientId, List<string> selectedGrantTypes)
        {
            return await _clientService.SaveAllowedGrants(clientId, selectedGrantTypes);
        }
    }
}

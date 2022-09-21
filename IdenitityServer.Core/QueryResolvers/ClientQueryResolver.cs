using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.QueryResolvers
{
    public class ClientQueryResolver
    {
        private readonly IClientService _clientService;
        public ClientQueryResolver(IClientService clientService)
        {
            _clientService = clientService;
        }
        public async Task<List<ClientModel>> GetAllClients()
        {
            return await _clientService.GetAllClients();
        }

        public async Task<ClientModel> GetClientById(int clientId)
        {
            return await _clientService.GetClientById(clientId);
        }
    }
}

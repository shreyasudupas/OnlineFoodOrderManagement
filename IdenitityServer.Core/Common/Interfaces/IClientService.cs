using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientModel>> GetAllClients();
        Task<ClientModel> GetClientById(int clientId);
    }
}

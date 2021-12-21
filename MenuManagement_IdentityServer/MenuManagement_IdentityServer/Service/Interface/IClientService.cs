using MenuManagement_IdentityServer.Models;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service.Interface
{
    public interface IClientService
    {
        Task<DisplayAllClients> GetAllClient();
        Task<ClientViewModel> GetClientInformation(string ClientId);
        Task<ClientViewModel> SaveClientInformation(ClientViewModel model);
    }
}

using MenuManagement_IdentityServer.Models;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service.Interface
{
    public interface IClientService
    {
        bool DeleteClientSecret(DeleteClientSecret ClientSecret);
        Task<DisplayAllClients> GetAllClient();
        Task<ClientViewModel> GetClientInformation(string ClientId);
        Task<ClientViewModel> SaveClientInformation(ClientViewModel model);
        RedirectUrlViewModel AddClientRedirect(RedirectUrlViewModel model);
        ClientSecretViewModel SaveClientSecret(ClientSecretViewModel model);
        bool DeleteClientRedirectUrl(DeleteRedirectUrl model);
        AddCorsAllowedOriginViewModel AddClientOriginUrl(AddCorsAllowedOriginViewModel model);
        bool DeleteClientAllowedOrigin(DeleteClientAllowedOrigin model);
        AddPostLogoutRedirectUriViewModel AddPostLogoutRedirectUrl(AddPostLogoutRedirectUriViewModel model);
        bool DeletePostLogoutUri(DeletePostLogoutRedirectUri model);
        DeleteClientViewModel DeleteClient(string ClientId);
    }
}

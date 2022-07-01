using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Mediators.Login;
using IdenitityServer.Core.Mediators.Logout;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginCommand login);
        Task<PreLogoutResponse> PreLogout(LogoutQuery logoutCommand);
    }
}

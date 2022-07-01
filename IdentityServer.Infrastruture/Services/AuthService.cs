using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Mediators.Login;
using IdenitityServer.Core.Mediators.Logout;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class AuthService: IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;

        public AuthService(SignInManager<ApplicationUser> signInManger,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interaction)
        {
            _signInManger = signInManger;
            _userManager = userManager;
            _interaction = interaction;
        }
        public async Task<LoginResponse> Login(LoginCommand login)
        {
            var response = new LoginResponse();
            var result = await _signInManger.PasswordSignInAsync(login.Username, login.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(login.Username);

                await _signInManger.SignInAsync(user, false);

                if (!string.IsNullOrEmpty(login.ReturnUrl))
                {
                    response.RedirectRequired = true;
                }
            }
            else
            {
                response.Error = "Incorrect Username/Password";
            }
            return response;
        }

        public async Task<PreLogoutResponse> PreLogout(LogoutQuery logoutCommand)
        {
            var response = new PreLogoutResponse { LogoutId = logoutCommand.LogoutId , ShowLogoutPrompt = true };

            if(logoutCommand.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                response.ShowLogoutPrompt = false;
                return response;
            }

            var context = await _interaction.GetLogoutContextAsync(response.LogoutId);
            if(context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                response.ShowLogoutPrompt = false;
                return response;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return response;
        }
    }
}

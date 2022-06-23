using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Request;
using IdenitityServer.Core.Domain.Response;
using IdentityServer.Infrastruture.Database;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class AuthService: IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(SignInManager<ApplicationUser> signInManger,
            UserManager<ApplicationUser> userManager)
        {
            _signInManger = signInManger;
            _userManager = userManager;
        }
        public async Task<LoginResponse> Login(LoginRequest login)
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
            return response;
        }
    }
}

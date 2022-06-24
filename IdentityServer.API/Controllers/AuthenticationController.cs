using IdenitityServer.Core.Mediators.Login;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.API.Controllers
{
    public class AuthenticationController : BaseController
    {
        [HttpPost("/api/Authentication/Login")]
        public Task<LoginCommandDto> Login(LoginCommand command)
        {
            return Mediator.Send(command);
        }
    }
}

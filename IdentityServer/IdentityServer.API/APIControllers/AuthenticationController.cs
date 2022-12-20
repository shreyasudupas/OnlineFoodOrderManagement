using IdenitityServer.Core.Features.Login;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.API.APIControllers
{
    public class AuthenticationController : BaseController
    {
        //[HttpPost("/api/Authentication/Login")]
        //public async Task<IActionResult> Login(LoginCommand command)
        //{
        //    var result = await Mediator.Send(command);
        //    if(result.RedirectRequired && result.isSuccess)
        //    {
        //        var removeHost = "https://localhost:5006";
        //        command.ReturnUrl = command.ReturnUrl.Replace(removeHost, string.Empty, System.StringComparison.OrdinalIgnoreCase);
        //        return Redirect(command.ReturnUrl);
        //    }
        //    else
        //    {
        //        return Ok(result);
        //    }
        //}
    }
}

using IdenitityServer.Core.Mediators.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.API.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginCommand { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _mediator.Send(command);

                if(loginResult.isSuccess == false)
                    ModelState.AddModelError("", "Invalid Login");
                else
                {
                    return Redirect(command.ReturnUrl);
                }
            }
            return View();
        }
    }
}

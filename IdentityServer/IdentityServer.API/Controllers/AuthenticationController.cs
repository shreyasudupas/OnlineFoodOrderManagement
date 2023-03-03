using AutoMapper;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.Login;
using IdenitityServer.Core.Features.Logout;
using IdenitityServer.Core.Features.Register;
using IdenitityServer.Core.Features.Utility;
using IdentityModel;
using IdentityServer.API.Controllers.ViewModels;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator,
            IIdentityServerInteractionService interaction,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _mediator = mediator;
            _interaction = interaction;
            _signInManager = signInManager;
            _mapper = mapper;
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
                {
                    ModelState.AddModelError("", loginResult.Error);
                    command.isSuccess = false;
                }
                else if(loginResult.RedirectRequired == true)
                {
                    return Redirect(command.ReturnUrl);
                }
                else
                {
                    //if loggedin from IDS then redirect to this page
                    return RedirectToAction("Privacy");
                }
            }
            return View(command);
        }

        /// <summary>
        /// This is for Pre Logout Screen to show the consent page before logout when user login directly from IDS or push to logout post if external signout
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var result = await _mediator.Send(new LogoutQuery { LogoutId = logoutId, IsAuthenticated = User.Identity.IsAuthenticated });
            if(result.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(new LogoutInputModel { LogoutId = result.LogoutId });
            }
            return View(result);
        }

        /// <summary>
        /// When user is clicked on logout 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModel(model.LogoutId);

            //If logout from ID Server directly
            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();
            }

            //if logout from angular/react client then redirect it to postback call
            if (!string.IsNullOrEmpty(vm.PostLogoutRedirectUri))
            {
                return Redirect(vm.PostLogoutRedirectUri);
            }

            return RedirectToAction("Login");
        }

        public async Task<LoggedOutViewModel> BuildLogoutViewModel(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = false,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated != true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }
            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Privacy()
        {
            var claims = User.Claims
                .Select(c => new PrivacyViewModel
                {
                    Type = c.Type,
                    Value = c.Value
                })
                .ToList();

            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> Register(string returnUrl)
        {
            var result = await _mediator.Send(new RegisterQuery { ReturnUrl = returnUrl });
            var response = _mapper.Map<RegisterViewModel>(result);
            
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            if(ModelState.IsValid)
            {
                await _mediator.Send(command);

                if(command.Errors.Any())
                {
                    command.Errors.ForEach(e=> ModelState.AddModelError("",e));
                }
                else if(!string.IsNullOrEmpty(command.ReturnUrl))
                {
                    return Redirect(command.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("login");
                }
            }
            else
            {
                ModelState.AddModelError("", "Property missing");
                var (cities,states) = await _mediator.Send(new GetStateCityQuery());
                command.Cities = cities;
                command.States = states;
            }
            var response = _mapper.Map<RegisterViewModel>(command);

            return View(response);
        }

        [HttpGet]
        public IActionResult RegisterAsAdmin(string returnUrl)
        {
            var model = new RegisterAdminResponse();

            if (returnUrl != null)
                model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsAdmin(RegisterAdminResponse registerAdminResponse)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(new RegisterAdminQuery { 
                    registerAdminResponse = registerAdminResponse
                });

                if(response.Errors.Count > 0)
                {
                    response.Errors.ForEach(err => ModelState.AddModelError("", err));
                }else
                {
                    return RedirectToAction("login");
                }
            }else
            {
                ModelState.AddModelError("", "Property missing");
            }
            return View(registerAdminResponse);
        }
    }
}

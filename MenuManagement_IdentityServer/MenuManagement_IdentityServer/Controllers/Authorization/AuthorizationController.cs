using IdentityServer4.Services;
using IdentityServer4.Stores;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Controllers.Authorization
{
    public class AuthorizationController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AuthorizationController(SignInManager<ApplicationUser> signInManger,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interaction, IClientStore clientStore, IAuthenticationSchemeProvider schemeProvider, IEventService events)
        {
            _signInManger = signInManger;
            _userManager = userManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManger.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, false);

                if (result.Succeeded)
                {
                    var GetUser = await _userManager.FindByNameAsync(vm.Username);

                    await  _signInManger.SignInAsync(GetUser, vm.RememberMe);

                    if (vm.ReturnUrl == null)
                    {
                        //go to User Dashboard of IDS server
                        return RedirectToAction("Index", "HomeDashboard");
                    }
                    else
                    {
                        return Redirect(vm.ReturnUrl);
                    }
                }

                ModelState.AddModelError("", "Invalid Login");
            }
            

            return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = vm.Username,
                    Email = vm.Email,
                    Address = vm.Address,
                    City = vm.City
                };
                
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    if(vm.ReturnUrl == null)
                    {
                        //if registering from Identity Server then go back to login
                        return RedirectToAction("Login", "Authorization");

                    }
                    else
                    {
                        //If registering from client then go back to screen will be asked to login later
                        return Redirect(vm.ReturnUrl);
                    }
                }
                
                //If Errors
                //if(result.Errors.Count() > 0)
                //{
                //    //vm.ErrorList = new List<string>();

                //    var error = result.Errors.ToList();
                //    error.ForEach(ele => {
                //        //vm.ErrorList.Add(ele.Description);
                //    });
                //}
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                return View(vm);
            }
            
            
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModel(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                //return await Logout(vm);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModel(model.LogoutId);

            //If logout from ID Server directly
            if(model.LogoutId == null)
            {
                // delete local authentication cookie
                //await HttpContext.SignOutAsync();

                await _signInManger.SignOutAsync();
            }
            
            return View("loggedOut",vm);
        }

        public async Task<LogoutViewModel> BuildLogoutViewModel(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if(User?.Identity.IsAuthenticated != true)
            {
                //if user is not authenticated, then show the logout page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult RedirectLink(string ReturnUrl)
        {
            var SplitUrl = ReturnUrl.Split('/');
            return RedirectToAction(SplitUrl[4],SplitUrl[3]);
        }
    }
}

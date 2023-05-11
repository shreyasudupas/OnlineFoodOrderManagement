using IdentityServer4.Extensions;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Controllers.User
{
    [Authorize]
    public class HomeDashboardController : Controller
    {
        private readonly IUserAdministrationManager userAdministration;
        public HomeDashboardController(IUserAdministrationManager userAdministration)
        {
            this.userAdministration = userAdministration;
        }

        public async Task<IActionResult> Index()
        {
            var UserId = User.GetSubjectId();

            var result = await userAdministration.GetUserDashBoardInformation(UserId);

            if(result.status == CrudEnumStatus.failure)
            {
                result.ErrorDescription.ForEach(err =>
                {
                    ModelState.AddModelError("", err);
                });
            }
            
            return View(result);
        }
    }
}

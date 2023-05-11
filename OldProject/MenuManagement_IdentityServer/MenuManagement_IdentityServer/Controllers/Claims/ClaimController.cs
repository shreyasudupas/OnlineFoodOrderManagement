using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MenuManagement_IdentityServer.Controllers.Claims
{
    [Authorize]
    public class ClaimController : Controller
    {
        private readonly IUserAdministrationManager _UserManager;
        public ClaimController(IUserAdministrationManager userManager)
        {
            _UserManager = userManager;
        }

        [HttpGet]
        public IActionResult GetListClaims()
        {
            var result = _UserManager.GetAllDropDownClaims();

            return View(result);
        }

        [HttpGet]
        public IActionResult EditClaimInSelectionDropDown(int? Id)
        {
            var result = _UserManager.GetClaimById(Id);
            if(result.status==CrudEnumStatus.failure)
            {
                result.ErrorDescription.ForEach(e => ModelState.AddModelError("",e));
            }

            return View(result);
        }

        [HttpPost]
        public IActionResult EditClaimInSelectionDropDown(EditClaimViewModel viewModel,string save)
        {
            EditClaimViewModel result = new EditClaimViewModel();
            if (ModelState.IsValid)
            {
                result = _UserManager.EditClaim(viewModel);
                if (result.status == CrudEnumStatus.failure)
                {
                    result.ErrorDescription.ForEach(error => ModelState.AddModelError("", error));
                }
            }
            else
            {
                var Errors = ModelState.Values.Where(e => e.Errors.Count > 0)
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                Errors.ForEach(e => ModelState.AddModelError("", e));
            }
            
            return View(result);
        }
    }
}

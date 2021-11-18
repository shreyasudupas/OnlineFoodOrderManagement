using AutoMapper;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Controllers.Administration
{
    //[Authorize]
    [Authorize(Roles = "admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserAdministrationManager _userAdministration;
        private readonly IUserRoleManager _userRoleManager;
        private readonly IMapper _mapper;

        public AdministrationController(
            RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager
            , IUserAdministrationManager userAdministration
            , IUserRoleManager userRoleManager
            , IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userAdministration = userAdministration;
            _userRoleManager = userRoleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var result = _userRoleManager.ListAllRoles();
            return View(result);
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public IActionResult AddRole()
        {
            return View(new AddRoleViewModel());
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _userRoleManager.AddRole(model);

                result.ErrorDescription.ForEach(error => 
                {
                    ModelState.AddModelError("", error);
                });
            }
            else
            {
                ModelState.AddModelError("", "Error in body, Please check");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string RoleId)
        {
            var result = await _userRoleManager.GetUserByNameRoleInformatation(RoleId);
            if(result.status == CrudEnumStatus.redirect)
            {
                //not found
                return Redirect("ListRoles");
            }
            else
            {
                EditRoleViewModel model = new EditRoleViewModel();
                model = _mapper.Map<EditRoleViewModel>(result);

                return View(model);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var result = await _userRoleManager.EditRole(model);
            if(result.status == CrudEnumStatus.redirect)
            {
                //in this case its success
                return RedirectToAction("ListRoles");
            }
            else if (result.status == CrudEnumStatus.NotFound)
            {
                ModelState.AddModelError("NotFound", $"Role with {model.RoleName} not found in the database. Please try another role name");
            }
            else if(result.status == CrudEnumStatus.failure)
            {
                //failure
                result.ErrorDescription.ForEach(err =>
                {
                    ModelState.AddModelError("", err);
                });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddUserRole(string RoleId)
        {
            TempData["RoleId"] = RoleId;

            var result = await _userRoleManager.GetUserRoleInformation(RoleId);
            if (result.Status == CrudEnumStatus.NotFound)
            {
                ModelState.AddModelError("NotFound", result.ErrorDescription);

                return PartialView("_AddUserRolePartial", result.ListUsersRole);
            }
            else
            {
                return PartialView("_AddUserRolePartial", result.ListUsersRole);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUserRole([FromBody]List<AddUserRoleModel> models)
        {
            //var sample  = new List<AddUserRoleModel>();
            //get the roleId
            string RoleId = string.Empty;
            if (TempData.ContainsKey("RoleId"))
                RoleId = TempData["RoleId"].ToString();

            TempData.Keep("RoleId");

            var result = await _userRoleManager.EditUserRoleInformation(RoleId,models);
            if(result.status == CrudEnumStatus.failure)
            {
                ModelState.AddModelError("", result.ErrorDescription);
            }

            return PartialView("_AddUserRolePartial", models);

         }

        [HttpGet]
        public IActionResult GetUserList()
        {
            var result = _userAdministration.GetAllApplicationUsers();
            return View("UserList",result);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string UserId)
        {
            var result = await _userAdministration.GetApplicationUserInfo(UserId);

            if(result.ErrorDescription != null)
            {
                return View(result.Users);
            }
            else
            {
                ModelState.AddModelError("", result.ErrorDescription);
                return View("Not Found");
            }


        }

        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser model)
        {
            if(ModelState.IsValid)
            {

                var result = await _userAdministration.EditUserInfo(model);

                if (result.ErrorDescription.Count > 0)
                {
                    result.ErrorDescription.ForEach(element =>
                    {
                        ModelState.AddModelError("", element);
                    });
                }
            }
            else
            {
                ModelState.AddModelError("", "Error in Form Details Please correct it and resubmit the form");
            }
            return View(model);
        }
    }
}

using MenuManagement_IdentityServer.Data.Models;
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
        private readonly IUserAdministration _userAdministration;

        public AdministrationController(
            RoleManager<IdentityRole> roleManager
            ,UserManager<ApplicationUser> userManager
            ,IUserAdministration userAdministration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userAdministration = userAdministration;
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
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
                IdentityRole identityRole = new IdentityRole { Name = model.RoleName };

                var result = await _roleManager.CreateAsync(identityRole);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);

            if(role == null)
            {
                //not found
                return Redirect("ListRoles");
            }

            var model = new EditRoleViewModel
            {
                Id = RoleId,
                RoleName = role.Name
            };

            //Find all users under the role
            foreach (var user in _userManager.Users.ToList())
            {
                var isRolePresent = _userManager.IsInRoleAsync(user, role.Name).Result;
                if (isRolePresent)
                {
                    model.UserNames.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                //not found
                //return Redirect("ListRoles");
                ModelState.AddModelError("NotFound", $"Role with {model.RoleName} not found in the database. Please try another role name");
                return View(model);
            }
            else
            {
                //update new role name
                role.Name = model.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddUserRole(string RoleId)
        {
            List<AddUserRoleModel> model = new List<AddUserRoleModel>();
            TempData["RoleId"] = RoleId;

            //Find if the RoleId belongs in Role table
            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role == null)
            {
                //not found
                //return Redirect("ListRoles");
                ModelState.AddModelError("NotFound", $"Role with {RoleId} Id is not found in the database. Please try another role name");
                return PartialView("_AddUserRolePartial", model);
            }
            else
            {
                foreach(var user in _userManager.Users.ToList())
                {
                    //if user is found with this role name then select the checkbox with this user
                    if(await _userManager.IsInRoleAsync(user,role.Name))
                    {
                        model.Add(new AddUserRoleModel
                        {
                            IsSelected = true,
                            UserId = user.Id,
                            Username = user.UserName
                        });
                    }else
                    {
                        //if user is not found with this role name then select do not select checkbox with this user
                        model.Add(new AddUserRoleModel
                        {
                            IsSelected = false,
                            UserId = user.Id,
                            Username = user.UserName
                        });
                    }
                }
                return PartialView("_AddUserRolePartial", model);
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

            if(!string.IsNullOrEmpty(RoleId))
            {
                var role = await _roleManager.FindByIdAsync(RoleId);
                if(role != null)
                {
                    //get all users under this role
                    var UsersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

                    //itertate through all the user got from modal
                    foreach (var model in models)
                    {
                        //find the user is present in db
                        var UserFromModal = await _userManager.FindByIdAsync(model.UserId);

                        if (UserFromModal == null)
                        {
                            ModelState.AddModelError("NotFound", $"User with {model.UserId} Id not found in the database. Please try another role name");
                        }
                        else
                        {
                            var UserWithExistingRole = UsersInRole.FirstOrDefault(user => user.Id == UserFromModal.Id);

                            //If the UserRole is not present in UserWithExistingRole and is selected then add
                            if ((UserWithExistingRole == null) && (model.IsSelected == true))
                            {
                                await _userManager.AddToRoleAsync(UserFromModal, role.Name);
                            }
                            //If the UserRole is present in UserWithExistingRole and the selection is false ,Remove the user with that role
                            else if ((UserWithExistingRole != null) && (model.IsSelected == false))
                            {
                                await _userManager.RemoveFromRoleAsync(UserFromModal, role.Name);
                            }
                            //If the UserRole is present in UserWithExistingRole and the selection is true ,Then do nothing or if nor user is found with that role and user is not selected
                            else if (((UserWithExistingRole != null) && (model.IsSelected == true) ) || ((UserWithExistingRole == null) && (model.IsSelected == false))) 
                            {
                                continue;
                            }

                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("NotFound", $"Role with {RoleId} Id not found in the database. Please try another role name");
                    return PartialView("_AddUserRolePartial", models);
                }
                
            }

            
             return PartialView("_AddUserRolePartial", models);

         }

        [HttpGet]
        public IActionResult GetUserList()
        {
            var Users = _userManager.Users;
            return View("UserList",Users);
        }
        [HttpGet]
        public IActionResult EditUser(string UserId)
        {
            var User = _userManager.Users.Where(x => x.Id == UserId).FirstOrDefault();
            if(User!= null)
            {
                return View(User);
            }
            else
            {
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

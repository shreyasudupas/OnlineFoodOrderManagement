using AutoMapper;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using MenuManagement_IdentityServer.Utilities.DropdownItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Controllers.Administration
{
    //[Authorize]
    public class AdministrationController : Controller
    {
        private readonly IUserAdministrationManager _userAdministration;
        private readonly IUserRoleManager _userRoleManager;
        private readonly IMapper _mapper;

        public AdministrationController(
            IUserAdministrationManager userAdministration
            , IUserRoleManager userRoleManager
            , IMapper mapper)
        {
            _userAdministration = userAdministration;
            _userRoleManager = userRoleManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult ListRoles()
        {
            var result = _userRoleManager.ListAllRoles();
            return View(result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AddRole()
        {
            return View(new AddRoleViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public IActionResult GetUserList()
        {
            var result = _userAdministration.GetAllApplicationUsers();
            return View("UserList",result);
        }
        [HttpGet]
        [Authorize(Roles = "admin,appUser")]
        public async Task<IActionResult> EditUser(string UserId)
        {
            var result = await _userAdministration.GetApplicationUserInfo(UserId);

            if (result.ErrorDescription.Count == 0)
            {
                return View(result);
            }
            else
            {
                result.ErrorDescription.ForEach(err =>
                {
                    ModelState.AddModelError("", err);
                });
                
                return View("Not Found");
            }


        }

        [HttpPost]
        [Authorize(Roles = "admin,appUser")]
        public async Task<IActionResult> EditUser(UserInfomationModel model,string save,string cancel)
        {
            EditUserGet editUserGetViewModel = new EditUserGet();
            //editUserGetViewModel.Users = model;

            if(!string.IsNullOrEmpty(cancel))
            {
                var previosPageUrl = TempData.Peek("PreviousUrl").ToString();
                return Redirect(Url.Content(previosPageUrl));

            }
            if(ModelState.IsValid)
            {
                if(!string.IsNullOrEmpty(save))
                {
                    editUserGetViewModel = await _userAdministration.EditUserInfo(model);

                    if (editUserGetViewModel.ErrorDescription.Count > 0)
                    {
                        editUserGetViewModel.ErrorDescription.ForEach(element =>
                        {
                            ModelState.AddModelError("", element);
                        });
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Error in Form Details Please correct it and resubmit the form");
            }
            return View(editUserGetViewModel);
        }

        [HttpGet]
        public IActionResult UserAddressPartialView(GetUserAddressModel model)
        {
            
            if (!string.IsNullOrEmpty(model.UserId) && model.UserAddressId < 1)
            {
                return PartialView("_UserAddressPartialView", new UserAddressPartialViewModel
                {
                    Cities = SelectListUtility.GetCityItems(),
                    States = SelectListUtility.GetStateItems(),
                    UserId = model.UserId
                });
            }
            else
            {
                var result = _userAdministration.GetUserAddressInformation(model);
                return PartialView("_UserAddressPartialView", result);
            }
            
        }

        [HttpPost]
        public IActionResult UserAddressPartialView(UserAddressPartialViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = _userAdministration.SaveUserAddress(model);
                if(result.status != CrudEnumStatus.success)
                {
                    result.ErrorDescription.ForEach(e =>
                    {
                        ModelState.AddModelError("",e);
                    });
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter Required Fields");
            }
            return PartialView("_UserAddressPartialView", model);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ManageUserRoles(string UserId)
        {
            ManagerUserRole result = new ManagerUserRole();
            if (!string.IsNullOrEmpty(UserId))
            {
                result = await _userAdministration.GetManageRoleInformation(UserId);

                if(result.status == CrudEnumStatus.failure)
                {
                    result.ErrorDescription.ForEach(err => {
                        ModelState.AddModelError("", err);
                    });
                    
                }
            }
            else
            {
                ModelState.AddModelError("", "User is not present");
            }
            return PartialView("_ManageUserRolesPartial",result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ManageUserRoles([FromBody]List<ManageUserPost> model)
        {
            var result = await _userAdministration.SaveManageRoleInformation(model);
            if(result.ErrorDescription.Count > 0 )
            {
                result.ErrorDescription.ForEach(err =>
                {
                    ModelState.AddModelError("",err);
                });
                
            }
            return PartialView("_ManageUserRolesPartial", result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult ManageUserClaim(string UserId)
        {
            ManagerUserClaimViewModel model = new ManagerUserClaimViewModel();
            if (!string.IsNullOrEmpty(UserId))
            {
                var result = _userAdministration.ManageUserClaimGet(UserId);

                model.SelectionUserClaims = result.UserClaims;
                model.UserClaimValue = "";
                model.UserId = UserId;

                if (result.status == CrudEnumStatus.failure)
                {
                    ModelState.AddModelError("", result.ErrorDescription);  

                }
            }
            else
            {
                ModelState.AddModelError("", "User is not present");
            }
            return PartialView("_ManageManageUserClaimPartial", model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ManageUserClaim(ManagerUserClaimViewModelPost model)
        {
            if(ModelState.IsValid)
            {
                var result = await _userAdministration.ManageUserClaimPost(model);
                if(result.ErrorDescription.Count > 0)
                {
                    result.ErrorDescription.ForEach(error=>
                    {
                        ModelState.AddModelError("",error);
                    });
                    
                }
                return PartialView("_ManageManageUserClaimPartial", result);
            }
            else
            {
                ManagerUserClaimViewModel Mucm = new ManagerUserClaimViewModel();
                ModelState.AddModelError("","Error in modal");
                return PartialView("_ManageManageUserClaimPartial", Mucm);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUserClaim(string UserId)
        {
            List<DeleteUserClaimViewModel> model = new List<DeleteUserClaimViewModel>();
            //Send this Id to Post method
            TempData["UserId"] = UserId;

            var result = await _userAdministration.GetDeleteUserClaimInfo(UserId);

            if(result.ErrorDescription.Count > 0)
            {
                result.ErrorDescription.ForEach(err=>
                {
                    ModelState.AddModelError("",err);
                });
            }
            else
            {
                var UserClaimsMap = _mapper.Map<List<DeleteUserClaimViewModel>>(result.UserClaims);
                model = UserClaimsMap;
            }

            return PartialView("_DeleteUserClaim",model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUserClaim([FromBody]List<DeleteUserClaimViewModel> model)
        {
            var UserId = TempData["UserId"].ToString();
            var result = await _userAdministration.DeleteUserClaimInfo(model,UserId);

            if (result.status == CrudEnumStatus.failure)
            {
                result.ErrorDescription.ForEach(err =>
                {
                    ModelState.AddModelError("", err);
                });
            }

            return PartialView("_DeleteUserClaim", model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Upload(IFormFile file)
        //{
        //    //using var image = Image.Load(file.OpenReadStream());
        //    //image.Mutate(x => x.Resize(256, 256));
        //    //image.Save("...");
        //    var userId = TempData["UserId"].ToString();
        //    //var response = await _userAdministration.ImageUpload(file,userId);
        //    return RedirectToAction("EditUser",new { UserId = userId });
        //}

    }
}

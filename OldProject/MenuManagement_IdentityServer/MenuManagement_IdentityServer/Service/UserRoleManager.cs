using MenuManagement_IdentityServer.Controllers.Administration;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service
{
    public class UserRoleManager : IUserRoleManager
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleManager(RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IEnumerable<IdentityRole> ListAllRoles()
        {
            var roles = _roleManager.Roles;
            return roles;
        }

        public async Task<AddRole> AddRole(AddRoleViewModel model)
        {
            AddRole addRole = new AddRole();
            addRole.RoleName = model.RoleName;

            IdentityRole identityRole = new IdentityRole { Name = model.RoleName };

            var result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded){}

            foreach (var error in result.Errors)
            {
                addRole.ErrorDescription.Add(error.Description);
            }
            return addRole;
        }

        public async Task<EditRoleGet> GetUserByNameRoleInformatation(string RoleId)
        {
            EditRoleGet editRoleGet = new EditRoleGet();
            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role == null)
            {
                //not found
                editRoleGet.status = CrudEnumStatus.redirect;
                return editRoleGet;
            }

            editRoleGet.Id = RoleId;
            editRoleGet.RoleName = role.Name;

            //Find all users under the role
            foreach (var user in _userManager.Users.ToList())
            {
                var isRolePresent = _userManager.IsInRoleAsync(user, role.Name).Result;
                if (isRolePresent)
                {
                    editRoleGet.Username.Add(user.UserName);
                }
            }
            editRoleGet.status = CrudEnumStatus.success;
            return editRoleGet;
        }

        public async Task<EditRolePost> EditRole(EditRoleViewModel model)
        {
            EditRolePost editRolePost = new EditRolePost();

            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                //not found
                editRolePost.ErrorDescription.Add($"Role with {model.RoleName} not found in the database. Please try another role name");
                editRolePost.status = CrudEnumStatus.NotFound;
                return editRolePost;
            }
            else
            {
                //update new role name
                role.Name = model.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    editRolePost.status = CrudEnumStatus.redirect;
                    return editRolePost;
                }

                foreach (var error in result.Errors)
                {
                   editRolePost.ErrorDescription.Add(error.Description);
                }
                editRolePost.status = CrudEnumStatus.failure;

                return editRolePost;
            }
        }

        public async Task<AddUserRoleModelGet> GetUserRoleInformation(string RoleId)
        {
            AddUserRoleModelGet addUserRoleModelGet = new AddUserRoleModelGet();

            //Find if the RoleId belongs in Role table
            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role == null)
            {
                //not found
                addUserRoleModelGet.ErrorDescription = $"Role with {RoleId} Id is not found in the database. Please try another role name";
                addUserRoleModelGet.Status = CrudEnumStatus.NotFound;
                return addUserRoleModelGet;
            }
            else
            {
                foreach (var user in _userManager.Users.ToList())
                {
                    //if user is found with this role name then select the checkbox with this user
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        addUserRoleModelGet.ListUsersRole.Add(new AddUserRoleModel
                        {
                            IsSelected = true,
                            UserId = user.Id,
                            Username = user.UserName
                        });
                    }
                    else
                    {
                        //if user is not found with this role name then select do not select checkbox with this user
                        addUserRoleModelGet.ListUsersRole.Add(new AddUserRoleModel
                        {
                            IsSelected = false,
                            UserId = user.Id,
                            Username = user.UserName
                        });
                    }
                }
                addUserRoleModelGet.Status = CrudEnumStatus.success;
                return addUserRoleModelGet;
            }
        }

        public async Task<AddUserRoleModelPost> EditUserRoleInformation(string RoleId,List<AddUserRoleModel> models)
        {
            AddUserRoleModelPost addUserRoleModelPost = new AddUserRoleModelPost();

            if (!string.IsNullOrEmpty(RoleId))
            {
                var role = await _roleManager.FindByIdAsync(RoleId);
                if (role != null)
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
                            addUserRoleModelPost.ErrorDescription = $"User with {model.UserId} Id not found in the database. Please try another role name";
                            addUserRoleModelPost.status = CrudEnumStatus.failure;
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
                            else if (((UserWithExistingRole != null) && (model.IsSelected == true)) || ((UserWithExistingRole == null) && (model.IsSelected == false)))
                            {
                                continue;
                            }
                            addUserRoleModelPost.status = CrudEnumStatus.success;
                        }
                    }
                }
                else
                {
                    addUserRoleModelPost.ErrorDescription = $"Role with {RoleId} Id not found in the database. Please try another role name";
                    addUserRoleModelPost.status = CrudEnumStatus.failure;
                }
            }else
            {
                addUserRoleModelPost.ErrorDescription = $"RoleId {RoleId} is empty";
                addUserRoleModelPost.status = CrudEnumStatus.failure;
            }

            return addUserRoleModelPost;
        }
    }
}

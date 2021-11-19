﻿using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service
{
    public class UserAdministrationManager : IUserAdministrationManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAdministrationManager(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<EditUser> EditUserInfo(ApplicationUser user)
        {
            EditUser editUser = new EditUser();
            var GetUser = await _userManager.FindByIdAsync(user.Id);

            if (GetUser == null)
            {
                editUser.ErrorDescription.Add("User not found in database");
            }
            else
            {
                GetUser.UserName = user.UserName;
                GetUser.Email = user.Email;
                GetUser.PhoneNumber = user.PhoneNumber;
                GetUser.Address = user.Address;
                GetUser.City = user.City;
                GetUser.IsAdmin = user.IsAdmin;

                editUser.User = GetUser;

                var result = await _userManager.UpdateAsync(GetUser);
                if (result.Succeeded)
                {
                    editUser.User = user;
                    return editUser;
                }

                foreach (var err in result.Errors)
                {
                    editUser.ErrorDescription.Add(err.Description);
                }
            }
            return editUser;
        }

        public IEnumerable<ApplicationUser> GetAllApplicationUsers()
        {
            var Users = _userManager.Users;
            return Users;
        }

        public async Task<EditUserGet> GetApplicationUserInfo(string Id)
        {
            EditUserGet editUser = new EditUserGet();

            var User = await _userManager.FindByIdAsync(Id);
            if(User == null)
            {
                editUser.ErrorDescription = "User not found in database";
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(User);
                var claims = await _userManager.GetClaimsAsync(User);

                editUser.Claims = claims.Select(x=>x.Value).ToList();
                editUser.Roles = roles;

                editUser.Users = User;
            }
            return editUser;
        }

        public async Task<ManagerUserRole> GetManageRoleInformation(string UserId)
        {
            ManagerUserRole managerUser = new ManagerUserRole();
            managerUser.UserId = UserId;

            var userProfile = await _userManager.FindByIdAsync(UserId);

            if(userProfile == null)
            {
                managerUser.status = CrudEnumStatus.failure;
                managerUser.ErrorDescription.Add($"user with {UserId} not present in the database");
            }
            else
            {
                //Existing role of user
                var UserRole = await _userManager.GetRolesAsync(userProfile);
                var GetAllRoles = _roleManager.Roles.ToList();

                //if no roles then add
                GetAllRoles.ForEach(role =>
                {
                    //see if the role is present in UserRoles
                    var isRolePresent = UserRole.Any(x => x == role.Name);

                    managerUser.Role.Add(new UserRoleSelection { 
                        RoleName = role.Name,
                        IsSelected = isRolePresent
                    });
                });

                managerUser.status = CrudEnumStatus.success;
            }

            return managerUser;
        }
    }
}
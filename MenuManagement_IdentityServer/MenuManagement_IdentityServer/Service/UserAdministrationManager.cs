using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service
{
    public class UserAdministrationManager : IUserAdministrationManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAdministrationManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                editUser.Users = User;
            }
            return editUser;
        }
    }
}

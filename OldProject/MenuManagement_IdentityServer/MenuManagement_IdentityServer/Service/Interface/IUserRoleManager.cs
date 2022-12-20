using MenuManagement_IdentityServer.Controllers.Administration;
using MenuManagement_IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service.Interface
{
    public interface IUserRoleManager
    {
        public IEnumerable<IdentityRole> ListAllRoles();
        public Task<AddRole> AddRole(AddRoleViewModel model);
        public Task<EditRoleGet> GetUserByNameRoleInformatation(string RoleId);
        public Task<EditRolePost> EditRole(EditRoleViewModel model);
        public Task<AddUserRoleModelGet> GetUserRoleInformation(string RoleId);
        public Task<AddUserRoleModelPost> EditUserRoleInformation(string RoleId,List<AddUserRoleModel> models);
    }
}

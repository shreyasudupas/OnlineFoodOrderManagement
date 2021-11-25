using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service.Interface
{
    public interface IUserAdministrationManager
    {
        public IEnumerable<ApplicationUser> GetAllApplicationUsers();
        public Task<EditUserGet> GetApplicationUserInfo(string Id);
        public Task<EditUser> EditUserInfo(ApplicationUser user);
        Task<ManagerUserRole> GetManageRoleInformation(string UserId);
        Task<ManagerUserRole> SaveManageRoleInformation(List<ManageUserPost> model);
        ManagerUserClaim ManageUserClaimGet(string UserId);
        Task<ManagerUserClaimViewModel> ManageUserClaimPost(ManagerUserClaimViewModelPost model);
        Task<DeleteUserClaimGet> GetDeleteUserClaimInfo(string UserId);
        Task<DeleteUserClaimGet> DeleteUserClaimInfo(List<DeleteUserClaimViewModel> model, string UserId);
    }
}

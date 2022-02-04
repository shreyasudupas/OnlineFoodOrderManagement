using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service.Interface
{
    public interface IUserAdministrationManager
    {
        public IEnumerable<ApplicationUser> GetAllApplicationUsers();
        public Task<EditUserGet> GetApplicationUserInfo(string Id);
        public Task<EditUserGet> EditUserInfo(UserInfomationModel user);
        Task<ManagerUserRole> GetManageRoleInformation(string UserId);
        Task<ManagerUserRole> SaveManageRoleInformation(List<ManageUserPost> model);
        ManagerUserClaim ManageUserClaimGet(string UserId);
        Task<ManagerUserClaimViewModel> ManageUserClaimPost(ManagerUserClaimViewModelPost model);
        Task<DeleteUserClaimGet> GetDeleteUserClaimInfo(string UserId);
        Task<DeleteUserClaimGet> DeleteUserClaimInfo(List<DeleteUserClaimViewModel> model, string UserId);
        Task<UserDashboard> GetUserDashBoardInformation(string UserId);
        ClaimsViewModel GetAllDropDownClaims();
        EditClaimViewModel EditClaim(EditClaimViewModel viewModel);
        EditClaimViewModel GetClaimById(int? Id);
        UserInformationModel GetUserInformationDetail(string UserId);
    }
}

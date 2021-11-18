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
    }
}

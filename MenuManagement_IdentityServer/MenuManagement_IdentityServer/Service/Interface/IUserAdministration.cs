using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service.Interface
{
    public interface IUserAdministration
    {
        public Task<EditUser> EditUserInfo(ApplicationUser user);
    }
}

using IdenitityServer.Core.Domain.DBModel;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserProfile> GetUserInformationById(string UserId);
        Task<UserProfile> GetUserClaims(UserProfile user);
        Task<UserProfile> GetUserRoles(UserProfile user);
    }
}

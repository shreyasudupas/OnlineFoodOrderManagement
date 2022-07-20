using IdenitityServer.Core.Domain.DBModel;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserProfile> GetUserInformationById(string UserId);
    }
}

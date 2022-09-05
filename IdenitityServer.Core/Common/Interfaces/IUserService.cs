using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserProfile> GetUserInformationById(string UserId);
        Task<UserProfile> GetUserClaims(UserProfile user);
        Task<UserProfile> GetUserRoles(UserProfile user);
        Task<List<DropdownModel>> GetCityById(int StateId);
        Task<List<DropdownModel>> GetLocationAreaById(int CityId);
        Task<bool> ModifyUserInformation(UserProfile user);
        Task<UserProfileAddress> AddModifyUserAddress(string UserId, UserProfileAddress profileAddress);
        Task<List<UserProfile>> GetUserList();
    }
}

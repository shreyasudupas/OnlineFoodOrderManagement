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
        Task<List<AddressDropdownModel>> GetAllCities();

        Task<VendorUserIdMapping> AddVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping);
        Task<List<VendorUserIdMapping>> GetAllVendorUserIdMapping(string? vendorId = null);

        Task<UserClaimModel> AddUserClaimsBasedOnUserId(UserClaimModel userClaimModel);
        Task<UserClaimModel> ModifyUserClaimsBasedOnUserId(UserClaimModel userClaimModel);
        Task<string> GetUserIdByVendorClaim(string vendorId);
    }
}

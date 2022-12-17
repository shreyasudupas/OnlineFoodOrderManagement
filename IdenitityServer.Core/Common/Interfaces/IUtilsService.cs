using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.Domain.Response;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IUtilsService
    {
        List<SelectListItem> GetAllStates();
        List<SelectListItem> GetAllCities();
        List<DropdownModel> GetAllScopeList();
        List<DropdownModel> GetAllowedScopeList();
        Task<UserProfile> GetUserProfile(string userId);
        Task<string> UpdateUserProfileImage(string userId, string newImagePath);
        Task<List<RegisteredLocationReponse>> GetAllRegisteredLocation();
        Task<ApiResourceScopeModel> AddApiResourceScope(int scopeId, int apiResourceId);
        Task<bool> DeleteApiResourceScope(string scopeName, int apiResourceId);
    }
}

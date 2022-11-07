using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IUtilsService
    {
        List<SelectListItem> GetAllStates();
        List<SelectListItem> GetAllCities();
        List<DropdownModel> GetAllowedScopeList();
        Task<UserProfile> GetUserProfile(string userId);
        Task<string> UpdateUserProfileImage(string userId, string newImagePath);
    }
}

using IdenitityServer.Core.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IUtilsService
    {
        List<SelectListItem> GetAllStates();
        List<SelectListItem> GetAllCities();
        List<DropdownModel> GetAllowedScopeList();
    }
}

using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Infrastruture.Services
{
    public class UtilsService : IUtilsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ConfigurationDbContext _configurationDbContext;

        public UtilsService(ApplicationDbContext context,
            ConfigurationDbContext configurationDbContext)
        {
            _context = context;
            _configurationDbContext = configurationDbContext;
        }
        public List<SelectListItem> GetAllStates()
        {
            var result =  _context.States.Select(state => new SelectListItem
            {
                Text = state.Name,
                Value = state.Name
            }).ToList();

            return result;
        }

        public List<SelectListItem> GetAllCities()
        {
            var result = _context.Cities.Select(state => new SelectListItem
            {
                Text = state.Name,
                Value = state.Name
            }).ToList();

            return result;
        }

        public List<DropdownModel> GetAllowedScopeList()
        {
            var result = _configurationDbContext.ApiScopes.Select(x => new DropdownModel
            {
                Label = x.Name,
                Value = x.Name
            }).ToList();

            result.AddRange(new List<DropdownModel>
            {
                new DropdownModel {  Label="profile" , Value = "profile" },
                new DropdownModel { Label="openid" , Value = "openid" }
            });

            return result;
        }
    }
}

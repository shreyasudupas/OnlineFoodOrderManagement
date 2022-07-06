using IdenitityServer.Core.Common.Interfaces;
using IdentityServer.Infrastruture.Database;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Infrastruture.Services
{
    public class UtilsService : IUtilsService
    {
        private readonly ApplicationDbContext _context;

        public UtilsService(ApplicationDbContext context)
        {
            _context = context;
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
    }
}

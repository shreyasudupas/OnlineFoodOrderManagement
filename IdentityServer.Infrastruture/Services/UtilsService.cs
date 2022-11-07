using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<UserProfile> GetUserProfile(string userId)
        {
            var users = await _context.Users.Where(u => u.Id == userId).Select(u=> new UserProfile
            {
                Id = u.Id,
                ImagePath = u.ImagePath
            }).FirstOrDefaultAsync();

            if (userId != null)
            {
                return users;
            }
            else
                return null;
        }

        public async Task<string> UpdateUserProfileImage(string userId,string newImageName)
        {
            var users = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (userId != null)
            {
                var oldImagePath = users.ImagePath;
                users.ImagePath = newImageName;
                _context.SaveChanges();
                return oldImagePath;
            }
            else
                return "";
        }
    }
}

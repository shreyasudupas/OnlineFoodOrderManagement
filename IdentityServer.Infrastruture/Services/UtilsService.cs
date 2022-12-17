using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.Domain.Response;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class UtilsService : IUtilsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly ILogger<UtilsService> _logger;

        public UtilsService(ApplicationDbContext context,
            ConfigurationDbContext configurationDbContext,
            ILogger<UtilsService> logger)
        {
            _context = context;
            _configurationDbContext = configurationDbContext;
            _logger = logger;
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

        public List<DropdownModel> GetAllScopeList()
        {
            var result = _configurationDbContext.ApiScopes.Select(x => new DropdownModel
            {
                Label = x.Name,
                Value = x.Name
            }).ToList();

            var resources = _configurationDbContext.ApiResources.Select(x => new DropdownModel
            {
                Label = x.Name,
                Value = x.Name
            }).ToList();

            result.AddRange(new List<DropdownModel>
            {
                new DropdownModel {  Label="profile" , Value = "profile" },
                new DropdownModel { Label="openid" , Value = "openid" }
            });

            result.AddRange(resources);

            return result;
        }

        public List<DropdownModel> GetAllowedScopeList()
        {
            var result = _configurationDbContext.ApiScopes.Select(x => new DropdownModel
            {
                Label = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return result;
        }

        public async Task<ApiResourceScopeModel> AddApiResourceScope(int scopeId,int apiResourceId)
        {
            var apiScope = _configurationDbContext.ApiScopes.Where(a => a.Id == scopeId).FirstOrDefault();
            if(apiScope != null)
            {
                var apiResource = await _configurationDbContext.ApiResources.Include(a=>a.Scopes).Where(r => r.Id == apiResourceId).FirstOrDefaultAsync();
                if(apiResource != null)
                {
                    apiResource.Scopes.Add(new ApiResourceScope
                    {
                        Scope= apiScope.Name,
                        ApiResourceId = apiResource.Id
                    });

                    _configurationDbContext.SaveChanges();

                    var updatedResource = _configurationDbContext.ApiResources.Where(r => r.Id == apiResourceId).Select(x => x.Scopes).FirstOrDefault();
                    if (updatedResource != null)
                    {
                        return updatedResource.Where(x => x.Scope == apiScope.Name && x.ApiResourceId == apiResourceId).Select(x => new ApiResourceScopeModel
                        {
                            Id= x.Id,
                            Scope = x.Scope,
                            ApiResourceId = x.ApiResourceId
                        }).FirstOrDefault();
                    }else
                    {
                        _logger.LogError("Error in retrieving updated apiResources");
                        return null;
                    }
                }
                else
                {
                    _logger.LogError($"Error in retrieving apiResource with Id {apiResourceId}");
                    return null;
                }
            }else
            {
                _logger.LogError($"AddAPiResourceScope with Id:{ scopeId} not present");
                return null;
            }
        }

        public async Task<bool> DeleteApiResourceScope(string scopeName, int apiResourceId)
        {
            var apiResource = await _configurationDbContext.ApiResources.Include(a=>a.Scopes).Where(r => r.Id == apiResourceId).FirstOrDefaultAsync();
            if (apiResource != null)
            {
                var itemToBeDeleted = apiResource.Scopes.Find(s => s.Scope == scopeName &&  s.ApiResourceId == apiResource.Id);
                if(itemToBeDeleted != null)
                {
                    apiResource.Scopes.Remove(itemToBeDeleted);

                    _configurationDbContext.SaveChanges();
                    return true;
                }
                else
                {
                    _logger.LogError($"Item with scope {scopeName} not found");
                    return false;
                }
            }
            else
            {
                _logger.LogError($"DeleteAPiResource-> ApiResource with Id:{ apiResourceId} not present");
                return false;
            }
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

        public async Task<List<RegisteredLocationReponse>> GetAllRegisteredLocation()
        {
            List<RegisteredLocationReponse> registeredLocationReponses = new List<RegisteredLocationReponse>();
            var locations = await _context.States.Include(s => s.Cities).ThenInclude(c=>c.Areas).ToListAsync();

            locations.ForEach(item => registeredLocationReponses.Add(new RegisteredLocationReponse
            {
                State = item
            }));

            return registeredLocationReponses;
        }
    }
}

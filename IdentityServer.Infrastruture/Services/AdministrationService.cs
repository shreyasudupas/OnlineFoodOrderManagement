using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.Domain.Response;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;

        public AdministrationService(
            RoleManager<IdentityRole> roleManager
            , ILogger<AdministrationService> logger
            , ApplicationDbContext applicationDbContext
            ,ConfigurationDbContext configurationDbContext)
        {
            _roleManager = roleManager;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _configurationDbContext = configurationDbContext;
        }

        public List<RoleListResponse> Roles()
        {
            var roles =  _roleManager.Roles.Select(r=> new RoleListResponse
            {
                RoleId = r.Id,
                RoleName = r.Name
            }).ToList();

            return roles;
        }

        public async Task<RoleListResponse> AddRole(RoleListResponse role)
        {
            var newRole = new IdentityRole { Name = role.RoleName };
            var response = await _roleManager.CreateAsync(newRole);
            if(response.Succeeded)
            {
                var roleInfo = _applicationDbContext.Roles.Where(r=>r.Name == role.RoleName)
                .Select(r=>new RoleListResponse{
                    RoleId = r.Id,
                    RoleName = r.Name
                })
                .FirstOrDefault();
                
                return roleInfo;
            }
            else
            {
                _logger.LogError($"Role Save Errors: {JsonConvert.SerializeObject(response.Errors)}");
                return null;
            }
        }

        public RoleListResponse GetRoleById(string RoleId)
        {
            var role = _roleManager.Roles.Where(r=>r.Id == RoleId).Select(r=> new RoleListResponse{
                RoleId = r.Id,
                RoleName = r.Name
            }).FirstOrDefault();

            if(role != null)
            {
                return role;
            }
            else
            {
                _logger.LogError($"Role with {RoleId} not present");
                return null;
            }
        }

        public RoleListResponse EditRoleById(RoleListResponse role)
        {
            var roleIfPresent = _applicationDbContext.Roles.Where(r => r.Id == role.RoleId).FirstOrDefault();

            if(roleIfPresent!=null)
            {
                roleIfPresent.Name = role.RoleName;
                _applicationDbContext.SaveChanges();

                _logger.LogInformation($"Role with Id{roleIfPresent.Id} changed");
                return role;
            }
            else
            {
                _logger.LogError($"Role with Id:{role.RoleId} is not present");
                return null;
            }

            
        }

        public async Task<RoleListResponse> DeleteRole(string roleId)
        {
            var result = new RoleListResponse();
            if(!string.IsNullOrEmpty(roleId))
            {
                var rolePresent = _roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefault();
                if (rolePresent != null)
                {
                    var role = _applicationDbContext.Roles.Remove(rolePresent);
                    await _applicationDbContext.SaveChangesAsync();

                    result.RoleId = rolePresent.Id;
                    result.RoleName = rolePresent.Name;

                    return result;
                }
                else
                {
                    _logger.LogError($"{roleId} is not present in the database");
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        public async Task<List<ApiScopeModel>> GetApiScopes()
        {
            var apiScopes = await _configurationDbContext.ApiScopes.Select(a=>new ApiScopeModel
            {
                Id = a.Id,
                Name = a.Name,
                DisplayName = a.DisplayName,
                Description = a.Description
            }).ToListAsync();

            return apiScopes;
        }

        public async Task<ApiScopeModel> GetApiScopeById(int Id)
        {
            var apiScope = await _configurationDbContext.ApiScopes.Where(a=>a.Id == Id).Select(a => new ApiScopeModel
            {
                Id = a.Id,
                Name = a.Name,
                DisplayName = a.DisplayName,
                Description = a.Description??""
            }).FirstOrDefaultAsync();

            if (apiScope != null)
                return apiScope;
            else
            {
                _logger.LogError($"ApiScope with Id {Id} not present");
                return null;
            }
        }

        public async Task<ApiScopeModel> AddApiScope(ApiScopeModel apiScope)
        {
            var isApiscope = await _configurationDbContext.ApiScopes.Where(a => a.Id == apiScope.Id)
                .FirstOrDefaultAsync();

            if (isApiscope == null)
            {
                _configurationDbContext.ApiScopes.Add(new ApiScope
                {
                    Name = apiScope.Name,
                    Description = apiScope.Description,
                    DisplayName = apiScope.DisplayName
                });

                await _configurationDbContext.SaveChangesAsync();

                apiScope.Id = _configurationDbContext.ApiScopes.Where(x => x.Name == apiScope.Name).Select(a => a.Id).FirstOrDefault();

                return apiScope;

            }else
            {
                _logger.LogError($"ApiScope with Id {apiScope.Id} is not present in database");
                return null;
            }
        }

        public async Task<ApiScopeModel> SaveApiScope(ApiScopeModel apiScope)
        {
            var isApiscope = await _configurationDbContext.ApiScopes.Where(a => a.Id == apiScope.Id)
                .FirstOrDefaultAsync();

            if (isApiscope != null)
            {
                isApiscope.Name = apiScope.Name;
                isApiscope.DisplayName = apiScope.DisplayName;
                isApiscope.Description = apiScope.Description;

                await _configurationDbContext.SaveChangesAsync();

                return apiScope;

            }
            else
            {
                _logger.LogError($"ApiScope with Id {apiScope.Id} is not present in database");
                return null;
            }
        }

        public async Task<ApiScopeModel> DeleteApiScope(int apiScopeId)
        {
            var isApiscope = await _configurationDbContext.ApiScopes.Where(a => a.Id == apiScopeId)
                .FirstOrDefaultAsync();
            if(isApiscope != null)
            {
                var remove = _configurationDbContext.ApiScopes.Remove(isApiscope);
                await _configurationDbContext.SaveChangesAsync();
                _logger.LogInformation($"ApiScope Id : {apiScopeId} Removed");
                return new ApiScopeModel 
                {
                    Id = isApiscope.Id,
                    Name = isApiscope.Name,
                    DisplayName = isApiscope.DisplayName,
                    Description = isApiscope.Description
                };
            }
            else
            {
                _logger.LogError($"ApiScope Id : {apiScopeId} not found in database");
                return null;
            }
        }
    }
}

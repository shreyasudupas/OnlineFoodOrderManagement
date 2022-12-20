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

        public async Task<List<ApiResourceModel>> GetAllApiResources()
        {
            var apiResources = await _configurationDbContext.ApiResources.Select(res => new ApiResourceModel
            {
                Id = res.Id,
                Enabled = res.Enabled,
                Name = res.Name,
                DisplayName = res.DisplayName,
                Description = res.Description,
                AllowedAccessTokenSigningAlgorithms = res.AllowedAccessTokenSigningAlgorithms,
                ShowInDiscoveryDocument = res.ShowInDiscoveryDocument,
                Created = res.Created.ToString("dd/MM/yyyy H:m:ss"),
                LastAccessed = (res.LastAccessed != null) ? res.LastAccessed.Value.ToString("dd/MM/yyyy H:m:ss") : "",
                NonEditable = res.NonEditable,
            }).ToListAsync();

            return apiResources;
        }

        public async Task<ApiResourceModel> GetApiResourceById(int id)
        {
            var resource = await _configurationDbContext.ApiResources.Include(x=>x.Scopes).Where(r => r.Id == id).FirstOrDefaultAsync();

            if(resource != null)
            {
                return new ApiResourceModel
                {
                    Id = resource.Id,
                    Enabled = resource.Enabled,
                    Name = resource.Name,
                    DisplayName = resource.DisplayName,
                    Description = resource.Description,
                    AllowedAccessTokenSigningAlgorithms = resource.AllowedAccessTokenSigningAlgorithms,
                    ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                    Created = resource.Created.ToString("dd/MM/yyyy H:m:ss"),
                    LastAccessed = (resource.LastAccessed != null) ? resource.LastAccessed.Value.ToString("dd/MM/yyyy H:m:ss") : "",
                    NonEditable = resource.NonEditable,
                    Scopes = resource.Scopes.Select(s=> new ApiResourceScopeModel
                    {
                        ApiResourceId = s.ApiResourceId, Id = s.Id , Scope = s.Scope
                    }).ToList()
                };
            }else
            {
                _logger.LogError($"ApiReource with Id {id} is not present");
                return null;
            }
        }

        public async Task<ApiResourceModel> AddApiResource(ApiResourceModel apiResourceModel)
        {
            var resource = await _configurationDbContext.ApiResources.Where(r => r.Id == apiResourceModel.Id).FirstOrDefaultAsync();

            if (resource == null)
            {
                var newApiResource = new ApiResource
                {
                    Name = apiResourceModel.Name,
                    Enabled = true,
                    DisplayName = apiResourceModel.DisplayName,
                    Description = apiResourceModel.Description,
                    NonEditable = true,
                    ShowInDiscoveryDocument = apiResourceModel.ShowInDiscoveryDocument,
                    AllowedAccessTokenSigningAlgorithms = apiResourceModel.AllowedAccessTokenSigningAlgorithms,
                    Created = System.DateTime.Now
                };

                _configurationDbContext.ApiResources.Add(newApiResource);
                

                //then add in IdentityResources as well
                //_configurationDbContext.IdentityResources.Add(new IdentityResource
                //{
                //    Enabled = true,
                //    Name = apiResourceModel.Name,
                //    DisplayName = apiResourceModel.DisplayName,
                //    Description = apiResourceModel.Description,
                //    Required = true,
                //    Emphasize =true,
                //    ShowInDiscoveryDocument = apiResourceModel.ShowInDiscoveryDocument,
                //    Created = System.DateTime.Now,
                //    NonEditable = apiResourceModel.NonEditable
                //});

                _configurationDbContext.SaveChanges();

                var newResource = _configurationDbContext.ApiResources.Where(a => a.Name == apiResourceModel.Name)
                    .Select(resource=> new ApiResourceModel
                    {
                        Id = resource.Id,
                        Enabled = resource.Enabled,
                        Name = resource.Name,
                        DisplayName = resource.DisplayName,
                        Description = resource.Description,
                        AllowedAccessTokenSigningAlgorithms = resource.AllowedAccessTokenSigningAlgorithms,
                        ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                        Created = resource.Created.ToString("dd/MM/yyyy H:m:ss"),
                        LastAccessed = (resource.LastAccessed != null)?resource.LastAccessed.Value.ToString("dd/MM/yyyy H:m:ss"):"",
                        NonEditable = resource.NonEditable,
                    }).FirstOrDefault();

                if (newResource != null)
                    return newResource;
                else
                {
                    _logger.LogError($"Resource with Name {apiResourceModel.Name} not found");
                    return null;
                }
            }
            else
            {
                _logger.LogInformation($"AddApiResource with Id {apiResourceModel.Id} is present in database");
                resource.Name = apiResourceModel.Name;
                resource.DisplayName = apiResourceModel.DisplayName;
                resource.Description = apiResourceModel.Description;
                resource.Enabled = apiResourceModel.Enabled;
                resource.AllowedAccessTokenSigningAlgorithms = apiResourceModel.AllowedAccessTokenSigningAlgorithms;
                resource.ShowInDiscoveryDocument = apiResourceModel.ShowInDiscoveryDocument;
                resource.NonEditable = apiResourceModel.NonEditable;

                _configurationDbContext.SaveChanges();
                _logger.LogInformation($"AddApiResource with Id {apiResourceModel.Id} Edit Saved successful");

                return new ApiResourceModel
                {
                    Id = resource.Id,
                    Enabled = resource.Enabled,
                    Name = resource.Name,
                    DisplayName = resource.DisplayName,
                    Description = resource.Description,
                    AllowedAccessTokenSigningAlgorithms = resource.AllowedAccessTokenSigningAlgorithms,
                    ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                    Created = resource.Created.ToString("dd/MM/yyyy H:m:ss"),
                    LastAccessed = (resource.LastAccessed != null) ? resource.LastAccessed.Value.ToString("dd/MM/yyyy H:m:ss") : "",
                    NonEditable = resource.NonEditable,
                };
            }
        }

        public async Task<ApiResourceModel> DeleteApiResourceById(int id)
        {
            var resource = await _configurationDbContext.ApiResources.Where(r => r.Id == id).FirstOrDefaultAsync();

            if (resource != null)
            {
                var res = _configurationDbContext.ApiResources.Remove(resource);

                //var identityResource = await _configurationDbContext.IdentityResources.Where(i => i.Name == resource.Name).FirstOrDefaultAsync();
                //if(identityResource != null)
                //{
                //    _configurationDbContext.IdentityResources.Remove(identityResource);
                //}

                _configurationDbContext.SaveChanges();

                _logger.LogInformation($"ApiResource with Id:{id} removed");
                return new ApiResourceModel
                {
                    Id = resource.Id,
                    Enabled = resource.Enabled,
                    Name = resource.Name,
                    DisplayName = resource.DisplayName,
                    Description = resource.Description,
                    AllowedAccessTokenSigningAlgorithms = resource.AllowedAccessTokenSigningAlgorithms,
                    ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                    Created = resource.Created.ToString("dd/MM/yyyy H:m:ss"),
                    LastAccessed = (resource.LastAccessed != null) ? resource.LastAccessed.Value.ToString("dd/MM/yyyy H:m:ss") : "",
                    NonEditable = resource.NonEditable,
                };
            }
            else
            {
                _logger.LogError($"ApiReource with Id {id} is not present");
                return null;
            }
        }

        public async Task<List<IdentityResourceModel>> GetAllIdentityResource()
        {
            var result = await _configurationDbContext.IdentityResources.Select(resource => new IdentityResourceModel
            {
                Id = resource.Id,
                Enabled = resource.Enabled,
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Required = resource.Required,
                Empahasize = resource.Emphasize,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                Created = resource.Created.ToString("dd/MM/yyyy hh:m:ss"),
                NonEditable = resource.NonEditable
            }).ToListAsync();

            return result;
        }

        public async Task<IdentityResourceModel> GetIdentityResourceById(int Id)
        {
            var result = await _configurationDbContext.IdentityResources.Where(x=>x.Id == Id).Select(resource => new IdentityResourceModel
            {
                Id = resource.Id,
                Enabled = resource.Enabled,
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Required = resource.Required,
                Empahasize = resource.Emphasize,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                Created = resource.Created.ToString("dd/MM/yyyy hh:m:ss"),
                NonEditable = resource.NonEditable
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IdentityResourceModel> AddIdentityResource(IdentityResourceModel identityResourceModel)
        {
            var identityResource = await _configurationDbContext.IdentityResources.Where(ir => ir.Id == identityResourceModel.Id).FirstOrDefaultAsync();
            if (identityResource == null)
            {
                _configurationDbContext.IdentityResources.Add(new IdentityResource
                {
                    Id = identityResourceModel.Id,
                    Enabled = identityResourceModel.Enabled,
                    Name = identityResourceModel.Name,
                    DisplayName = identityResourceModel.DisplayName,
                    Description = identityResourceModel.Description,
                    Required = identityResourceModel.Required,
                    Emphasize = identityResourceModel.Empahasize,
                    ShowInDiscoveryDocument = identityResourceModel.ShowInDiscoveryDocument,
                    Created = System.DateTime.Now,
                    NonEditable = identityResourceModel.NonEditable
                });

                _configurationDbContext.SaveChanges();

                var updatedModelId = await _configurationDbContext.IdentityResources.Where(ir => ir.Name == identityResourceModel.Name).FirstOrDefaultAsync();
                identityResourceModel.Id = updatedModelId.Id;

                return identityResourceModel;
            }
            else
            {
                _logger.LogInformation($"AddIdentityResource: Id {identityResourceModel.Id} already present");
                return identityResourceModel;
            }
        }

        public async Task<IdentityResourceModel> UpdateIdentityResource(IdentityResourceModel identityResourceModel)
        {
            var identityResource = await _configurationDbContext.IdentityResources.Where(ir => ir.Id == identityResourceModel.Id).FirstOrDefaultAsync();
            if (identityResource != null)
            {
                identityResource.Enabled = identityResourceModel.Enabled;
                identityResource.Name = identityResourceModel.Name;
                identityResource.DisplayName = identityResourceModel.DisplayName;
                identityResource.Description = identityResourceModel.Description;
                identityResource.Required = identityResourceModel.Required;
                identityResource.Emphasize = identityResourceModel.Empahasize;
                identityResource.ShowInDiscoveryDocument = identityResourceModel.ShowInDiscoveryDocument;
                identityResource.Updated = System.DateTime.Now;
                identityResource.NonEditable = identityResourceModel.NonEditable;

                _configurationDbContext.SaveChanges();

                return identityResourceModel;
            }
            else
            {
                _logger.LogInformation($"UpdateIdentityResource: Id {identityResourceModel.Id} not present in database");
                return identityResourceModel;
            }
        }

        public async Task<bool> DeleteIdentityResource(int id)
        {
            var identityResource = await _configurationDbContext.IdentityResources.Where(ir => ir.Id == id).FirstOrDefaultAsync();
            if (identityResource != null)
            {

                _configurationDbContext.IdentityResources.Remove(identityResource);
                _configurationDbContext.SaveChanges();

                return true;
            }
            else
            {
                _logger.LogInformation($"DeleteIdentityResource: Id {id} not present");
                return false;
            }
        }
    }
}

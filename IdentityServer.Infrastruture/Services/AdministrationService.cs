using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using Microsoft.AspNetCore.Identity;
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

        public AdministrationService(RoleManager<IdentityRole> roleManager
            , ILogger<AdministrationService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
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
                return role;
            }
            else
            {
                _logger.LogError($"Role Save Errors: {JsonConvert.SerializeObject(response.Errors)}");
                return null;
            }
        }
    }
}

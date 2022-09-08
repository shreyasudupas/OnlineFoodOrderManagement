﻿using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.EntityFramework.DbContexts;
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
        private readonly ApplicationDbContext _applicationDbContext;

        public AdministrationService(
            RoleManager<IdentityRole> roleManager
            , ILogger<AdministrationService> logger
            , ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
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

        public async Task<bool> DeleteRole(string roleId)
        {
            var rolePresent = _roleManager.Roles.Where(r=>r.Id==roleId).FirstOrDefault();
            if(rolePresent != null)
            {
                var role = _applicationDbContext.Roles.Remove(rolePresent);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                _logger.LogError($"{roleId} is not present in the database");
                return false;
            }
        }
    }
}

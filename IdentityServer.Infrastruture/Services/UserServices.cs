using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.MapperProfiles.Custom;
using IdentityServer.Infrastruture.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class UserServices : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserServices(
            ApplicationDbContext context,
            ILogger<UserServices> logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserProfile> GetUserInformationById(string UserId)
        {
            var user = await _context.Users.Include(u=>u.Address).Where(u => u.Id == UserId).FirstOrDefaultAsync();
            if(user != null)
            {
                //var modelMap = _mapper.Map<UserProfile>(user);
                var modelMap = user.MapToProfile(_context);
                return modelMap;
            }
            else
            {
                _logger.LogInformation($"{UserId} is not present in the database");
                return null;
            }
        }

        public async Task<UserProfile> GetUserClaims(UserProfile user)
        {
            if(user is not null)
            {
                if (!string.IsNullOrEmpty(user.Id))
                {
                    var appUser = new ApplicationUser
                    {
                        Id = user.Id
                    };

                    var userClaims = await _userManager.GetClaimsAsync(appUser);

                    if (userClaims.Count > 0)
                    {
                        user.Claims = new Dictionary<string, string>();
                        foreach (var claim in userClaims)
                        {
                            user.Claims.Add(claim.Type, claim.Value);
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Claims not present for userId: {user.Id}");
                    }
                }
                else
                {
                    _logger.LogInformation($"{user.Id} is empty in GetUserClaims function");
                }
                return user;

            }
            else
            {
                _logger.LogInformation($"user is null");
            }
            return null;
        }

        public async Task<UserProfile> GetUserRoles(UserProfile user)
        {
            if (user is not null)
            {
                if (!string.IsNullOrEmpty(user.Id))
                {
                    var appUser = new ApplicationUser
                    {
                        Id = user.Id
                    };

                    var userRoles = await _userManager.GetRolesAsync(appUser);

                    if (userRoles.Count > 0)
                    {
                        int i = 0;
                        user.Roles = new Dictionary<int, string>();
                        foreach (var role in userRoles)
                        {
                            user.Roles.Add(++i, role);
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Roles not present for userId: {user.Id}");
                    }
                }
                else
                {
                    _logger.LogInformation($"{user.Id} is empty in GetUserRoles function");
                }
                return user;

            }
            else
            {
                _logger.LogInformation($"user is null");
            }
            return null;
        }

        public async Task<List<DropdownModel>> GetCityById(int StateId)
        {
            if(StateId > 0)
            {
                var response = await _context.Cities.Where(c=>c.StateId == StateId).Select(city => new DropdownModel
                {
                    Label = city.Name,
                    Value = city.Id
                }).ToListAsync();

                return response;
            }
            else
            {
                _logger.LogInformation($"user is null");
                return null;
            }
            
        }

        public async Task<List<DropdownModel>> GetLocationAreaById(int CityId)
        {
            if (CityId > 0)
            {
                var response = await _context.LocationAreas.Where(c => c.CityId == CityId).Select(city => new DropdownModel
                {
                    Label = city.AreaName,
                    Value = city.Id
                }).ToListAsync();

                return response;
            }
            else
            {
                _logger.LogInformation($"user is null");
                return null;
            }
            
        }
    }
}

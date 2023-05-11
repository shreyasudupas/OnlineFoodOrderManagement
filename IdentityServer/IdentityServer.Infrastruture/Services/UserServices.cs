using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.MapperProfiles.Custom;
using IdentityServer.Infrastruture.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
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
                        foreach (var claim in userClaims)
                        {
                            user.Claims.Add(new DropdownModel{
                                Label = claim.Type,
                                Value = claim.Value
                            });
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
                        foreach (var role in userRoles)
                        {
                            user.Roles.Add(new DropdownModel{
                                Label = role,
                                Value = role
                            });
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

        public async Task<List<AddressDropdownModel>> GetAllCities()
        {
            var response = await _context.Cities.Include(c=>c.Areas).Select(city => new AddressDropdownModel
            {
                Label = city.Name,
                Value = city.Id.ToString(),
                Items = city.Areas.Select(a=> new DropdownModel {
                    Label = a.AreaName,
                    Value = a.Id.ToString()
                }).ToList()
            }).ToListAsync();

            return response;

        }

        public async Task<List<DropdownModel>> GetCityById(int StateId)
        {
            if(StateId > 0)
            {
                var response = await _context.Cities.Where(c=>c.StateId == StateId).Select(city => new DropdownModel
                {
                    Label = city.Name,
                    Value = city.Id.ToString()
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
                    Value = city.Id.ToString()
                }).ToListAsync();

                return response;
            }
            else
            {
                _logger.LogInformation($"user is null");
                return null;
            }
            
        }

        public async Task<bool> ModifyUserInformation(UserProfile user)
        {
            var userIdPresent = await _userManager.GetUserIdAsync(new ApplicationUser { Id = user.Id });

            if(!string.IsNullOrEmpty(userIdPresent))
            {
                var currentUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
                
                currentUser.Email = user.Email;
                currentUser.CartAmount = user.CartAmount;
                currentUser.Points = user.Points;
                //currentUser.IsAdmin = user.IsAdmin;
                currentUser.Enabled = user.Enabled;
                currentUser.UserType = user.UserType;

                var result = await _userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    //update the claims
                    var claimEnabled = await _context.UserClaims.Where(uc=>uc.ClaimType == "enabled" && uc.UserId == currentUser.Id).FirstOrDefaultAsync();
                    if(claimEnabled != null)
                    {
                        claimEnabled.ClaimValue = user.Enabled.ToString();
                    }

                    var claimEmail = await _context.UserClaims.Where(uc => uc.ClaimType == "email" && uc.UserId == currentUser.Id).FirstOrDefaultAsync();
                    if (claimEmail != null)
                    {
                        claimEmail.ClaimValue = user.Email;
                    }
                    _context.SaveChanges();

                    return result.Succeeded;
                }
                else
                {
                    _logger.LogError(JsonConvert.SerializeObject(result.Errors));
                    return false;
                }
            }else
            {
                _logger.LogInformation($"User with {user.Id} not present in database");
                return false;
            }
        }

        public async Task<UserProfileAddress> AddModifyUserAddress(string UserId,UserProfileAddress profileAddress)
        {
            var userInfo = await _context.Users.Include(u => u.Address).Where(u => u.Id == UserId).FirstOrDefaultAsync();

            if(userInfo != null)
            {
                var addressToModify = _context.UserAddresses.Where(x=>x.Id == profileAddress.Id && x.ApplicationUserId == UserId)
                    .FirstOrDefault();

                if(addressToModify != null)
                {
                    addressToModify.FullAddress = profileAddress.FullAddress;
                    addressToModify.City = _context.Cities.Where(c=>c.Id == Convert.ToInt32(profileAddress.CityId)).Select(c=>c.Name).FirstOrDefault();
                    addressToModify.Area = _context.LocationAreas.Where(c => c.Id == Convert.ToInt32(profileAddress.AreaId)).Select(c => c.AreaName).FirstOrDefault();
                    addressToModify.State = _context.States.Where(c => c.Id == Convert.ToInt32(profileAddress.StateId)).Select(c => c.Name).FirstOrDefault();
                    addressToModify.IsActive = profileAddress.IsActive;
                }
                else
                {
                    //rest of the address becomes false and new address becomes active
                    foreach (var address in userInfo.Address)
                    {
                        address.IsActive = false;
                    }

                    userInfo.Address.Add(new UserAddress
                    {
                        Area = _context.LocationAreas.Where(c => c.Id == Convert.ToInt32(profileAddress.AreaId)).Select(c => c.AreaName).FirstOrDefault(),
                        City = _context.Cities.Where(c => c.Id == Convert.ToInt32(profileAddress.CityId)).Select(c => c.Name).FirstOrDefault(),
                        FullAddress = profileAddress.FullAddress,
                        IsActive = true,
                        State = _context.States.Where(c => c.Id == Convert.ToInt32(profileAddress.StateId)).Select(c => c.Name).FirstOrDefault()
                    });
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("User is not present");
                return null;
            }
            return profileAddress;
        }

        public async Task<List<UserProfile>> GetUserList()
        {
            var users = new List<UserProfile>();

            var user = await _context.Users.ToListAsync();
            if (user.Count > 0)
            {
                user.ForEach(u =>
                {
                    users.Add(u.MapToProfile(_context));
                });
            }
            else
            {
                _logger.LogInformation($"Users is not present in the database");
                return null;
            }
            return users;
        }

        public async Task<VendorUserIdMapping> AddVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping)
        {
            if(!_context.VendorUserIdMappings.Any(v=>v.UserId == vendorUserIdMapping.UserId))
            {
                await _context.VendorUserIdMappings.AddAsync(vendorUserIdMapping);
                return vendorUserIdMapping;
            }
            else
            {
                _logger.LogInformation($"{vendorUserIdMapping.UserId} is already present");
                return null;
            }
        }

        public async Task<List<VendorUserIdMapping>> GetAllVendorUserIdMapping(string? vendorId = null)
        {
            var allUsers = from vendorIdMap in _context.VendorUserIdMappings
                           select vendorIdMap;

            if (vendorId != null)
                allUsers.Where(v => v.VendorId == vendorId);

            var list = allUsers.ToList();

            return list;
        }
    }
}

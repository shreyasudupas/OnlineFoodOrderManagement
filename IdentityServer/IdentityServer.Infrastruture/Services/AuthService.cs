using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.Login;
using IdenitityServer.Core.Features.Logout;
using IdenitityServer.Core.Features.Register;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class AuthService: IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<AuthService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IAddressService _addressService;

        public AuthService(SignInManager<ApplicationUser> signInManger,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interaction,
            ILogger<AuthService> logger,
            ApplicationDbContext context,
            IAddressService addressService)
        {
            _signInManger = signInManger;
            _userManager = userManager;
            _interaction = interaction;
            _logger = logger;
            _context = context;
            _addressService = addressService;
        }
        public async Task<LoginResponse> Login(LoginCommand login)
        {
            var response = new LoginResponse();
            var result = await _signInManger.PasswordSignInAsync(login.Username, login.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(login.Username);

                await _signInManger.SignInAsync(user, false);

                if (!string.IsNullOrEmpty(login.ReturnUrl))
                {
                    response.RedirectRequired = true;
                }
            }
            else
            {
                response.Error = "Incorrect Username/Password";
            }
            return response;
        }

        public async Task<PreLogoutResponse> PreLogout(LogoutQuery logoutCommand)
        {
            var response = new PreLogoutResponse { LogoutId = logoutCommand.LogoutId , ShowLogoutPrompt = true };

            if(logoutCommand.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                response.ShowLogoutPrompt = false;
                return response;
            }

            var context = await _interaction.GetLogoutContextAsync(response.LogoutId);
            if(context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                response.ShowLogoutPrompt = false;
                return response;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return response;
        }

        public async Task Register(RegisterCommand reqisterCommand)
        {
            //reqisterCommand.Cities = _context.Cities.Select(c=>new SelectListItem {
            //    Text = c.Name,
            //    Value = c.Name
            //}).ToList();

            var getState = _context.States.Where(s => s.Id == Convert.ToInt32(reqisterCommand.StateId)).Select(s => s.Name).FirstOrDefault();
            var getCity = _context.Cities.Where(s => s.Id == Convert.ToInt32(reqisterCommand.CityId)).Select(s => s.Name).FirstOrDefault();
            var getArea = _context.LocationAreas.Where(s => s.Id == Convert.ToInt32(reqisterCommand.AreaId)).Select(s => s.AreaName).FirstOrDefault();

            reqisterCommand.States = _context.States.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            }).ToList();

            var user = new ApplicationUser
            {
                UserName = reqisterCommand.Username,
                Email = reqisterCommand.Email,
                CreatedDate = DateTime.Now,
                Enabled= true,
                Address = new List<UserAddress>
                    {
                        new UserAddress
                        {
                            FullAddress = reqisterCommand.Address,
                            City = getCity,
                            State = getState,
                            Area = getArea,
                            IsActive = true
                        }
                    }
            };

            var result = await _userManager.CreateAsync(user, reqisterCommand.Password);
            _logger.LogInformation($"user {reqisterCommand.Username} added in database {result.Succeeded}");

            if (result.Succeeded)
            {
                //add claims
                var usernameClaim = new Claim("userName", reqisterCommand.Username);
                var emailClaim = new Claim("email", reqisterCommand.Email);
                var addressClaim = new Claim("address", JsonConvert.SerializeObject(user.Address));
                var roleAddressClaim = new Claim("role", "user");


                var result1 = _userManager.AddClaimAsync(user, usernameClaim).GetAwaiter().GetResult();
                var result2 = _userManager.AddClaimAsync(user, emailClaim).GetAwaiter().GetResult();
                var result3 = _userManager.AddClaimAsync(user, addressClaim).GetAwaiter().GetResult();
                var resultRole = _userManager.AddClaimAsync(user, roleAddressClaim).GetAwaiter().GetResult();

                _logger.LogInformation($"Claim for username: {result1.Succeeded}, Claim for email: {result2.Succeeded}, Claim for address: {result3.Succeeded}");

                var roleUserResult = _userManager.AddToRoleAsync(user,"appUser").GetAwaiter().GetResult();
                _logger.LogInformation($"Role appUser added to user {roleUserResult.Succeeded}");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    reqisterCommand.Errors.Add(error.Description);
                });
            }
        }

        public async Task<RegisterAdminResponse> RegisterAdmin(RegisterAdminResponse registerAdminResponse)
        {
            var user = new ApplicationUser
            {
                UserName = registerAdminResponse.Username,
                Email = registerAdminResponse.Email,
                CreatedDate = DateTime.Now,
                Enabled = true,
                //IsAdmin = true,
            };

            var result = await _userManager.CreateAsync(user, registerAdminResponse.Password);
            _logger.LogInformation($"user {registerAdminResponse.Username} added in database {result.Succeeded}");

            if (result.Succeeded)
            {
                var usernameClaim = new Claim("userName", registerAdminResponse.Username);
                var emailClaim = new Claim("email", registerAdminResponse.Email);
                var roleClaim = new Claim("role", "admin");

                var result1 = _userManager.AddClaimAsync(user, usernameClaim).GetAwaiter().GetResult();
                var result2 = _userManager.AddClaimAsync(user, emailClaim).GetAwaiter().GetResult();
                var resultRole = _userManager.AddClaimAsync(user, roleClaim).GetAwaiter().GetResult();

                _logger.LogInformation($"Claim for username: {result1.Succeeded}, Claim for email: {result2.Succeeded}");

                var roleUserResult = _userManager.AddToRoleAsync(user, "admin").GetAwaiter().GetResult();
                _logger.LogInformation($"Role Admin Mapping: {roleUserResult.Succeeded}");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                    registerAdminResponse.Errors.Add(error.Description));

                _logger.LogError($"Error when registering admin user {JsonConvert.SerializeObject(result.Errors)}");
            }

            return registerAdminResponse;
        }

        public async Task<(VendorRegister,string)> RegisterAsVendor(VendorRegister vendorRegister)
        {
            var appUser = new ApplicationUser();

            var user = new ApplicationUser
            {
                UserName = vendorRegister.Username,
                Email = vendorRegister.Email,
                CreatedDate = DateTime.Now,
                Enabled = false,
                UserType = IdenitityServer.Core.Domain.Enums.UserTypeEnum.Vendor,
                //IsAdmin = true,
            };

            var result = await _userManager.CreateAsync(user, vendorRegister.Password);
            _logger.LogInformation($"user {vendorRegister.Username} added in database {result.Succeeded}");

            if (result.Succeeded)
            {
                var usernameClaim = new Claim("userName", vendorRegister.Username);
                var emailClaim = new Claim("email", vendorRegister.Email);
                var roleClaim = new Claim("role", "vendor");

                var result1 = await _userManager.AddClaimAsync(user, usernameClaim);
                var result2 = await _userManager.AddClaimAsync(user, emailClaim);
                var resultRole = await _userManager.AddClaimAsync(user, roleClaim);

                _logger.LogInformation($"Claim for username: {result1.Succeeded}, Claim for email: {result2.Succeeded}");

                var roleUserResult = _userManager.AddToRoleAsync(user, "vendor").GetAwaiter().GetResult();
                _logger.LogInformation($"Role Admin Mapping: {roleUserResult.Succeeded}");

                appUser = await _userManager.FindByNameAsync(vendorRegister.Username);
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                    vendorRegister.Errors.Add(error.Description));

                _logger.LogError($"Error when registering vendor user {JsonConvert.SerializeObject(result.Errors)}");
            }

            return (vendorRegister, appUser.Id);
        }

        public async Task<bool> AddVendorUserAddress(string userId, UserAddress userAddress)
        {
            bool status = false;
            var user = await _context.Users.Where(u=>u.Id == userId).Include(x=>x.Address).FirstOrDefaultAsync();

            if(user != null)
            {
                if (userAddress != null)
                {
                    user.Address.Add(userAddress);

                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            return status;
        }

        public async Task<UserAddress> GetVendorAddressByVendorId(string vendorId)
        {
            var vendorAddress = await _context.UserAddresses.Where(ua=>ua.VendorId == vendorId).FirstOrDefaultAsync();
            return vendorAddress;
        }

        public async Task<bool> AddVendorClaim(VendorUserIdMapping vendorUserIdMapping)
        {
            var user = await _userManager.FindByIdAsync(vendorUserIdMapping.UserId);

            if(user != null)
            {
                var vendorIdClaim = new Claim("vendorId", vendorUserIdMapping.VendorId);

                var result1 = await _userManager.AddClaimAsync(user, vendorIdClaim);
                _logger.LogInformation($"Claim for user: {vendorUserIdMapping.UserId}, vendorId success:{result1.Succeeded}");

                return true;
            }
            else
            {
                _logger.LogError($"user not found with id: {vendorUserIdMapping.UserId}");
                return false;
            }
        }
    }
}

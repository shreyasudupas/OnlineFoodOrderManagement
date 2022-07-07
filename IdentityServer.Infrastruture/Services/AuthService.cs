﻿using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.Login;
using IdenitityServer.Core.Features.Logout;
using IdenitityServer.Core.Features.Register;
using IdentityServer.Infrastruture.Database;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public AuthService(SignInManager<ApplicationUser> signInManger,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interaction,
            ILogger<AuthService> logger,
            ApplicationDbContext context)
        {
            _signInManger = signInManger;
            _userManager = userManager;
            _interaction = interaction;
            _logger = logger;
            _context = context;
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
            reqisterCommand.Cities = _context.Cities.Select(c=>new SelectListItem {
                Text = c.Name,
                Value = c.Name
            }).ToList();

            reqisterCommand.States = _context.States.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            }).ToList();

            var user = new ApplicationUser
            {
                UserName = reqisterCommand.Username,
                Email = reqisterCommand.Email,
                Address = new List<UserAddress>
                    {
                        new UserAddress
                        {
                            FullAddress = reqisterCommand.Address,
                            City = reqisterCommand.City,
                            State = reqisterCommand.State,
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
                var roleAddressClaim = new Claim("role", "appUser");


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
    }
}

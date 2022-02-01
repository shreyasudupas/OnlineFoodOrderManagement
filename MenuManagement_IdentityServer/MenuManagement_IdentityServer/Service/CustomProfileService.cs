using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MenuManagement_IdentityServer.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service
{
    public class CustomProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory
            , UserManager<ApplicationUser> userManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }

        // In the first method, we create a logic to include required claims for a user using the context object
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            claims.Add(new Claim("username", user.UserName));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}

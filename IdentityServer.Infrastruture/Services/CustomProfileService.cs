using IdentityServer.Infrastruture.Database;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    /// <summary>
    /// This is used to add custom properties on access token, so that external client can access API that belongs to IDS
    /// </summary>
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
            //var principal = await _claimsFactory.CreateAsync(user);

            //var claims = principal.Claims.ToList();
            //claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            //Find user role claims
            var claims = await _userManager.GetClaimsAsync(user);
            var claimList = claims.ToList();
            var customClaimList = new List<Claim>();

            if (claimList.Any(x => x.Type == "role"))
            {
                var role = claimList.Find(x => x.Type == "role");
                customClaimList.Add(new Claim("role", role.Value));
            }

            if (claimList.Any(x => x.Type == "userName"))
            {
                var role = claimList.Find(x => x.Type == "userName");
                customClaimList.Add(new Claim("Username", role.Value));
            }

            customClaimList.Add(new Claim("UserId", user.Id));

            context.IssuedClaims = customClaimList;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}

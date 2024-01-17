using IdenitityServer.Core.Features.Utility;
using IdenitityServer.Core.Features.Vendor.Commands;
using IdenitityServer.Core.Features.Vendor.Query.IsVendorEnabledQuery;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.APIControllers
{
    [Authorize(LocalApi.PolicyName)]
    public class VendorController : BaseController
    {
        [HttpGet("/api/vendor/enable/{userId}")]
        public async Task<bool> GetVendorEnabled(string userId)
        {
            return await Mediator.Send(new IsVendorEnabledQuery { UserId = userId });
        }

        [HttpPut("/api/vendor/update/enable")]
        public async Task<bool> UpdateUserEnable([FromBody] UpdateUserEnableCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("/api/vendor/add/vendorClaim")]
        public async Task<bool> AddVendorClaimToUser([FromBody] AddVendorIdToClaimCommand addVendorIdToClaimCommand)
        {
            return await Mediator.Send(addVendorIdToClaimCommand);
        }
    }
}

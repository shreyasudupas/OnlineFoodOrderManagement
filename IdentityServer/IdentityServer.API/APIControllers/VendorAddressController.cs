using IdenitityServer.Core.Features.VendorUserAddress.Command;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.APIControllers
{
    [Authorize(LocalApi.PolicyName)]
    public class VendorAddressController : BaseController
    {
        [HttpPost("/api/vendor-address")]
        public async Task<bool> AddVendorUserMapping([FromBody] RegisterVendorAddressCommand registerVendorAddressCommand)
        {
            return await Mediator.Send(registerVendorAddressCommand);
        }
    }
}

using IdenitityServer.Core.Features.Vendor.Query.IsVendorEnabledQuery;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.API.APIControllers
{
    [Authorize]
    public class VendorController : BaseController
    {
        [HttpGet("api/vendor/enable")]
        public async Task<bool> GetVendorEnabled(string userId)
        {
            return await Mediator.Send(new IsVendorEnabledQuery { UserId = userId });
        }
    }
}

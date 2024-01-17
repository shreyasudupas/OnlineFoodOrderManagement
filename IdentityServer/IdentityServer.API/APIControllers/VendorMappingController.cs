using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.VendorMapping.Commands.AddVendorUserMapping;
using IdenitityServer.Core.Features.VendorMapping.Commands.UpdateVendorUserMapping;
using IdenitityServer.Core.Features.VendorMapping.Query.CheckIfVendorsUserEnabled;
using IdenitityServer.Core.Features.VendorMapping.Query.GetAllVendorUserMapping;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.APIControllers
{
    [Authorize(LocalApi.PolicyName)]
    public class VendorMappingController : BaseController
    {
        [HttpGet("/api/vendor-user-mapping/all")]
        public async Task<List<VendorMappingResponse>> GetAllVendorUserMapping([FromQuery]string VendorId)
        {
            return await Mediator.Send(new GetAllVendorUserMappingQuery { VendorId = VendorId });
        }

        [HttpPost("/api/vendor-user-mapping")]
        public async Task<VendorMappingResponse> AddVendorUserMapping([FromBody] AddVendorUserMappingCommand addVendorUserMapping)
        {
            return await Mediator.Send(addVendorUserMapping);
        }

        [HttpPut("/api/vendor-user-mapping")]
        public async Task<VendorMappingResponse> UpdateVendorUserMapping([FromBody] UpdateVendorUserMappingCommand updateVendorUserMapping)
        {
            return await Mediator.Send(updateVendorUserMapping);
        }

        [HttpGet("/api/vendor-user-mapping/enabled")]
        public async Task<VendorUserMappingEnableResponse> IsVendorUserMappingEnabled([FromQuery] string vendorId,string userId)
        {
            return await Mediator.Send(new CheckIfVendorsUserEnabledQuery { VendorId = vendorId, UserId = userId });
        }
    }
}

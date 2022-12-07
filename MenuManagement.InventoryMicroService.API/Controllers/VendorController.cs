using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Commands.AddVendors;
using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorList;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    [Authorize]
    public class VendorController : BaseController
    {
        [AllowAnonymous]
        [HttpGet("/api/vendors")]
        public async Task<List<VendorDto>> GetAllVendors()
        {
            return await Mediator.Send(new VendorListQuery());
        }

        [AllowAnonymous]
        [HttpPost("/api/vendors")]
        public async Task<List<VendorDto>> AddVendorsList([FromBody]AddVendorsCommand addVendorsCommand)
        {
            return await Mediator.Send(addVendorsCommand);
        }
    }
}

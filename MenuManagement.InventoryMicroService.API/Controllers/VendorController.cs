using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorDetails;
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK
            ,Type = typeof(List<VendorDetailsResponseDto>))]
        public async Task<List<VendorDetailsResponseDto>> GetAllVendorDetails()
        {
            return await Mediator.Send(new GetVendorDetailsQuery());
        }
    }
}

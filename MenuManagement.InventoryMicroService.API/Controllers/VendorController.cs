﻿using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorDetails;
using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorMenuDetails;
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
        public async Task<List<VendorDetailsResponseDto>> GetAllVendorDetails(string location)
        {
            return await Mediator.Send(new GetVendorDetailsQuery { Locality = location });
        }

        [HttpGet("/api/vendor/menudetails")]
        [ProducesResponseType(StatusCodes.Status200OK
            , Type = typeof(VendorMenuDetailDto))]
        public async Task<VendorMenuDetailDto> GetVendorMenuDetails(GetVendorMenuDetailsQuery getVendorMenuDetailsQuery)
        {
            return await Mediator.Send(getVendorMenuDetailsQuery);
        }
    }
}
using MediatR;
using MenuInventory.Microservice.Features.VendorFeature.Querries;
using MicroService.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class VendorController : ControllerBase
    {
        //private readonly IVendorBL _vendor;

        //public VendorController(IVendorBL vendor)
        //{
        //    _vendor = vendor;
        //}
        private readonly IMediator _mediator;

        public VendorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public string GetVendor()
        {
            return "This is vendor Controller";
        }

        /// <summary>
        /// This is to get the Vendor List
        /// </summary>
        /// <returns>List of string</returns>
        /// <response code="404">No Vendors</response>
        /// <response code="500">Exception in code</response>
        [HttpGet]
        public async Task<IActionResult> GetVendorListAsync()
        {
            APIResponse aPIResponse = new APIResponse();
            //var getResult = await _vendor.GetVendorListAsync();
            var getResult = await _mediator.Send(new GetAllVendorRequest());
            if (getResult.Count > 0)
            {
                aPIResponse.Content = getResult;
                aPIResponse.Response = 200;
            }
            else
            {
                aPIResponse.Response = 404;
            }
            return Ok(aPIResponse);
        }
    }
}

using Identity.MicroService.Models.APIResponse;
using MediatR;
using MenuInventory.Microservice.Features.MenuFeature.Querries;
using Common.Utility.Models;
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
    public class MenuController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MenuController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public string SampleMenu()
        {
            return "MenuController Works";
        }

        /// <summary>
        /// Get the Menu List from the Vendor ID
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns>Menu list</returns>
        [HttpGet]
        public async Task<Response> GetMenuListAsync(string VendorId)
        {
            //var headers = HttpContext.Request.Headers["UserInfo"];

            var result = await _mediator.Send(new VendorIdRequest(VendorId));
            if (result.Data != null && result.MenuColumnData != null)
            {
                return new Response(System.Net.HttpStatusCode.OK, result, null);
            }
            else
            {
                return new Response(System.Net.HttpStatusCode.NotFound, null, null);
            }
        }
    }
}

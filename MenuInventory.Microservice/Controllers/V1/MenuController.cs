using MediatR;
using MenuInventory.Microservice.Features.MenuFeature.Querries;
using MicroService.Shared.Models;
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
        public async Task<IActionResult> GetMenuListAsync(int VendorId)
        {
            //var headers = HttpContext.Request.Headers["UserInfo"];

            APIResponse response = new APIResponse();
            //var result = await _menu.GetMenuListForVednorId(VendorId);
            var result = await _mediator.Send(new VendorIdRequest(VendorId));
            if (result != null)
            {
                response.Content = result;
                response.Response = 200;
            }
            else
            {
                response.Response = 404;
            }

            return Ok(response);
        }
    }
}

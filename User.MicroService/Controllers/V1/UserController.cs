using Identity.MicroService.Features.UserFeature.Queries;
using MediatR;
using MicroService.Shared.BuisnessLayer.IBuisnessLayer;
using MicroService.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.MicroService.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the user details and if not present then adds it
        /// </summary>
        /// <returns>User Profile Details</returns>
        /// <response code="200">success userDetails</response>
        [HttpPost]
        [AllowAnonymous]
        //[Authorize(Policy = "AllowUserAccess")]
        public async Task<IActionResult> GetOrUpdateUserDetails(GetUserRequestModel Username)
        {
            APIResponse response = new APIResponse();

            var res = await _mediator.Send(Username);
            if (res != null)
            {
                response.Content = res;
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

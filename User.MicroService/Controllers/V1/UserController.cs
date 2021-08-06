using Identity.MicroService.Features.UserFeature.Queries;
using Identity.MicroService.Models.APIResponse;
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
    [Authorize(Policy = "AllowUserAccess")]
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
        //[AllowAnonymous]
        public async Task<Response> GetOrUpdateUserDetails(GetUserRequestModel Username)
        {
            var res = await _mediator.Send(Username);
            if (res != null)
            {
                return new Response(System.Net.HttpStatusCode.OK, res, null);
            }
            else
            {
                return new Response(System.Net.HttpStatusCode.NotFound, null, null);
            }
        }

        /// <summary>
        /// Get the dropdown values
        /// </summary>
        /// <returns>List of drop down value if ok</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<Response> GetPaymentDropDown()
        {
            var res = await _mediator.Send(new GetPaymentDropdownValue());
            if (res.Count> 0)
            {
                return new Response(System.Net.HttpStatusCode.OK, res, null);
            }
            else
            {
                return new Response(System.Net.HttpStatusCode.NotFound, null, null);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Response> Authenticate(GetAuthorizationTokenForUser User)
        {
            var result = await _mediator.Send(User);
            return new Response(System.Net.HttpStatusCode.OK, result, null);
        }
    }
}

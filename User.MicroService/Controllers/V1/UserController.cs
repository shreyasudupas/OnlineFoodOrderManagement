using MicroService.Shared.BuisnessLayer.IBuisnessLayer;
using MicroService.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace User.MicroService.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        public UserController(IUser user)
        {
            _user = user;
        }
        /// <summary>
        /// Gets the user details and if not present then adds it
        /// </summary>
        /// <returns>User Profile Details</returns>
        /// <response code="200">success userDetails</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrUpdateUserDetails(UserProfile userProfile)
        {
            APIResponse response = new APIResponse();
            
            var res = await _user.AddOrGetUserDetails(userProfile);
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace MenuManagement_IdentityServer.ApiController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    [Authorize(Policy = "CommonRoleAccess")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUserInformation()
        {
            var userId = User.Claims.Where(x => x.Type == "sub").Select(x=>x.Value).FirstOrDefault();

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}

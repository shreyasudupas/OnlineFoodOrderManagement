using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.MicroService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public string CheckUser()
        {
            return "User controller called";
        }

        [HttpPost]
        public IActionResult PostUser([FromBody]Profile profile)
        {
            if (profile != null)
                return Ok("Name is" + profile.Name);
            else
              return NotFound();

        }

    }

    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

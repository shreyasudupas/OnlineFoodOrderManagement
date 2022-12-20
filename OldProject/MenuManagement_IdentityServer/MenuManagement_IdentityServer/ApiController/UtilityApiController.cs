using MenuManagement_IdentityServer.ApiController.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static IdentityServer4.IdentityServerConstants;

namespace MenuManagement_IdentityServer.ApiController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    [Authorize(Policy = "CommonRoleAccess")]
    public class UtilityController : ControllerBase
    {
        private readonly IUserAdministrationManager _userAdministrationManager;

        public UtilityController(IUserAdministrationManager userAdministrationManager)
        {
            _userAdministrationManager = userAdministrationManager;
        }

        [HttpGet]
        public List<LocationDropDown> GetLocationDropDown()
        {
            var userId = User.Claims.Where(x => x.Type == "sub").Select(x => x.Value).FirstOrDefault();
            return _userAdministrationManager.GetLocationDropDownForUi(userId);
        }
    }
}

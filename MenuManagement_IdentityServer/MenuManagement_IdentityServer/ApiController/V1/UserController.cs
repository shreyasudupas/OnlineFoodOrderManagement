using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static IdentityServer4.IdentityServerConstants;

namespace MenuManagement_IdentityServer.ApiController.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    [Authorize(Policy = "CommonRoleAccess")]
    public class UserController : ControllerBase
    {
        private readonly IUserAdministrationManager _UserService;

        public UserController(IUserAdministrationManager userService)
        {
            _UserService = userService;
        }

        [HttpGet]
        public APIResponse GetUserInformation()
        {
            APIResponse response = new APIResponse();
            var userId = User.Claims.Where(x => x.Type == "sub").Select(x=>x.Value).FirstOrDefault();

            var result = _UserService.GetUserInformationDetail(userId);
            if(result.status == CrudEnumStatus.success)
            {
                response.StatusCode = 200;
                response.Content = result;
            }
            else if(result.status == CrudEnumStatus.NotFound)
            {
                response.Exception = result.ErrorDescription;
                response.StatusCode = 404;
            }
            else
            {
                response.Exception = result.ErrorDescription;
                response.StatusCode = 500;
            }

            return response;
        }
    }
}

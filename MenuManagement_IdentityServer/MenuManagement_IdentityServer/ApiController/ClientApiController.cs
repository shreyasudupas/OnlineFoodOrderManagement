using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MenuManagement_IdentityServer.ApiController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientApiController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientApiController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public bool DeleteClientSecret(DeleteClientSecret deleteClient)
        {
            var result = _clientService.DeleteClientSecret(deleteClient);
            return result;
        }

        [HttpPost]
        public bool DeleteClientRedirectUrl(DeleteRedirectUrl model)
        {
            var result = _clientService.DeleteClientRedirectUrl(model);
            return result;
        }
        [HttpPost]
        public bool DeleteClientAllowedOrigin(DeleteClientAllowedOrigin model)
        {
            var result = _clientService.DeleteClientAllowedOrigin(model);
            return result;
        }
        [HttpPost]
        public bool DeleteClientPostLogoutUri(DeletePostLogoutRedirectUri model)
        {
            var result = _clientService.DeletePostLogoutUri(model);
            return result;
        }
    }
}

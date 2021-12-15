using AutoMapper;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Controllers.Clients
{
    [Authorize(Roles ="admin")]
    public class ClientController : Controller
    {
        private readonly ILogger logger;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(ILogger<ClientController> logger
            , IClientService clientService
            , IMapper mapper)
        {
            this.logger = logger;
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            GetAllClientsViewModel model = new GetAllClientsViewModel();

            logger.LogInformation("Display all Client called");
            var result = await _clientService.GetAllClient();

            model.ServiceData.Data = result.Data;

            if(result.ErrorDescription.Count > 0)
            {
                result.ErrorDescription.ForEach(error=> ModelState.AddModelError("",error));
                
            }
            logger.LogInformation("Display all Client ended");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditClientInformation(string ClientId)
        {
            var result = await _clientService.GetClientInformation(ClientId);
            if(result.status != CrudEnumStatus.success)
            {
                result.ErrorDescription.ForEach(error =>
                {
                    ModelState.AddModelError("", error);
                });
                
            }
            return View(result);
        }
    }
}

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

        [HttpPost]
        public async Task<IActionResult> EditClientInformation(ClientViewModel model,string save,string cancel)
        {
            logger.LogInformation("EditClientInformation all Client called");
            //If button clicked is cancel
            if(cancel != null)
            {
                logger.LogInformation("Redirect to All client page");
                return RedirectToAction("GetAllClients");
            }
            if (ModelState.IsValid)
            {
                var result = await _clientService.SaveClientInformation(model);
                if (result.status != CrudEnumStatus.success)
                {
                    result.ErrorDescription.ForEach(error =>
                    {
                        ModelState.AddModelError("", error);
                    });

                }
                logger.LogInformation("EditClientInformation all Client ended");
                return View(result);
            }
            else
            {
                ModelState.AddModelError("", "Model error");
                logger.LogInformation("EditClientInformation all Client ended");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ManageClientSecret(string ClientId)
        {
            return PartialView("_ManageClientSecretPartial",new ClientSecretViewModel {  ClientId = ClientId});
        }

        [HttpPost]
        public IActionResult ManageClientSecret(ClientSecretViewModel model)
        {
            logger.LogInformation("ManageClientSecret API called");
            if (ModelState.IsValid)
            {
                var result = _clientService.SaveClientSecret(model);
                if(result.status == CrudEnumStatus.failure)
                {
                    result.ErrorDescription.ForEach(error =>
                    {
                        ModelState.AddModelError("", error);
                    });
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter the Required Fields");
            }

            logger.LogInformation("ManageClientSecret API end");
            return PartialView("_ManageClientSecretPartial",model);
        }

        [HttpGet]
        public IActionResult AddRedirectUrl(string ClientId)
        {
            return PartialView("_RedirectUrlPartial",new RedirectUrlViewModel { ClientId =ClientId });
        }

        [HttpPost]
        public IActionResult AddRedirectUrl(RedirectUrlViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = _clientService.AddClientRedirect(model);
                if(result.status == CrudEnumStatus.failure)
                {
                    result.ErrorDescription.ForEach(err =>
                    {
                        ModelState.AddModelError("", err);
                    });
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter Reqired Fields");
            }
            return PartialView("_RedirectUrlPartial", model);
        }
        [HttpGet]
        public IActionResult AddCorsAllowedOrigin(string ClientId)
        {
            return PartialView("_AddCorsAllowedOriginPartial", new AddCorsAllowedOriginViewModel { ClientId = ClientId });
        }
        [HttpPost]
        public IActionResult AddCorsAllowedOrigin(AddCorsAllowedOriginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = _clientService.AddClientOriginUrl(model);
                if(result.status == CrudEnumStatus.failure)
                {
                    result.ErrorDescription.ForEach(error => ModelState.AddModelError("", error));
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter Required Fields");
            }
            return PartialView("_AddCorsAllowedOriginPartial", model);
        }

        [HttpGet]
        public IActionResult AddPostLogoutRedirectUri(string ClientId)
        {
            return PartialView("_AddPostLogoutRedirectUriPartial", new AddPostLogoutRedirectUriViewModel { ClientId = ClientId });
        }
        [HttpPost]
        public IActionResult AddPostLogoutRedirectUri(AddPostLogoutRedirectUriViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _clientService.AddPostLogoutRedirectUrl(model);
                if (result.status == CrudEnumStatus.failure)
                {
                    result.ErrorDescription.ForEach(error => ModelState.AddModelError("", error));
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter Required Fields");
            }
            return PartialView("_AddPostLogoutRedirectUriPartial", model);
        }

        public IActionResult DeleteClient(string ClientId)
        {
            logger.LogInformation($"DeleteClient API called :{ClientId}");
            var result = _clientService.DeleteClient(ClientId);
            if(result.status == CrudEnumStatus.success)
            {
                logger.LogInformation("DeleteClient API end");
                return RedirectToAction("GetAllClients", "Client");

            }
            else
            {
                logger.LogInformation("DeleteClient API end");
                return RedirectToAction("EditClientInformation", "Client", new { ClientId = ClientId });
            }
            
        }
    }
}

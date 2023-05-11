using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.Features.Register;
using IdenitityServer.Core.Features.Utility;
using IdenitityServer.Core.Features.VendorMapping.Commands.AddVendorUserMapping;
using IdenitityServer.Core.Features.VendorUserAddress.Command;
using IdenitityServer.Core.Features.VendorUserAddress.Queries;
using IdentityServer.API.Controllers.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API.Controllers
{
    public class VendorRegistrationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public VendorRegistrationController(IMediator mediator,
            ILogger<VendorRegistrationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register(string VendorId,string VendorName,string Email)
        {
            var model = new VendorUserRegistrationViewModel();
            model.VendorId = VendorId;
            model.Vendorname = VendorName;
            model.Email = Email;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(VendorUserRegistrationViewModel vendorUserRegistrationViewModel)
        {
            if (ModelState.IsValid)
            {
                var decryptVendorId = await _mediator.Send(new DencryptionServiceQuery {  ResponseStream = vendorUserRegistrationViewModel.VendorId });

                if (!string.IsNullOrEmpty(decryptVendorId))
                {
                    _logger.LogInformation($"Vendor Id Decrpyted sucessfully {decryptVendorId}");

                    var vendorResult = await _mediator.Send(new RegisterAsVendorCommand
                    {
                        vendorRegister = new VendorRegister
                        {
                            VendorName = vendorUserRegistrationViewModel.Vendorname,
                            Email = vendorUserRegistrationViewModel.Email,
                            Password = vendorUserRegistrationViewModel.Password,
                            Username = vendorUserRegistrationViewModel.Username,
                        },
                        ProcessQueue = false
                    });

                    if(!vendorResult.Errors.Any())
                    {
                        var vendorAddress = await _mediator.Send(new GetVendorAddressByVendorIdQuery
                        {
                            VendorId = decryptVendorId
                        });

                        if(vendorAddress != null)
                        {
                            _logger.LogInformation($"Vendor Address found values: {JsonConvert.SerializeObject(vendorAddress)}");

                            var addVendorAddress = await _mediator.Send(new RegisterVendorAddressCommand
                            {
                                UserId = vendorResult.UserId,
                                UserAddress = new IdenitityServer.Core.Domain.DBModel.UserAddress
                                {
                                    ApplicationUserId = vendorResult.UserId,
                                    VendorId = decryptVendorId,
                                    Area= vendorAddress.Area,
                                    City = vendorAddress.City,
                                    FullAddress = vendorAddress.FullAddress,
                                    Editable = false,
                                    IsActive = true,
                                    State = vendorAddress.State
                                }
                            });

                            _logger.LogInformation($"Vendor User Address added succesfully: {addVendorAddress}");

                            //next add vendorId mapping table
                            var vendorUserMappingResult = await _mediator.Send(new AddVendorUserMappingCommand
                            {
                                NewVendorUserMapping = new IdenitityServer.Core.Domain.Response.VendorMappingResponse
                                {
                                    Enabled = false,
                                    UserId = vendorResult.UserId,
                                    VendorId = decryptVendorId
                                }
                            });
                            
                        }
                        else
                        {
                            _logger.LogError($"Found no address for Exsiting Vendor in UserAddress");
                        }

                        //Redirect User
                        return RedirectToAction("ShowPostRegistration",new { VendorName = vendorUserRegistrationViewModel.Vendorname,
                            Username = vendorUserRegistrationViewModel.Username });
                    }
                    else 
                    {
                        vendorResult.Errors.ForEach(e => ModelState.AddModelError("", e));
                    }
                }
                else
                {
                    ModelState.AddModelError("","Vendor Id missing");
                }
            }else
            {
                ModelState.AddModelError("", "Property missing");
            }

            return View(vendorUserRegistrationViewModel);
        }

        [HttpGet]
        public IActionResult ShowPostRegistration(string VendorName,string Username)
        {
            var model = new PostRegisterViewModel();
            model.VendorName = VendorName;
            model.UserName = Username;
            return View(model);
        }
    }
}

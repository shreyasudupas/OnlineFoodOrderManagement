using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using MenuManagement.MessagingQueue.Core.Interfaces.Producers;
using MenuManagement.MessagingQueue.Core.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Register
{
    public class RegisterAsVendorCommand : IRequest<VendorRegister>
    {
        public VendorRegister vendorRegister { get; set; }
        public bool ProcessQueue { get; set; }
    }

    public class RegisterAsVendorCommandHandler : IRequestHandler<RegisterAsVendorCommand, VendorRegister>
    {
        private readonly IAuthService _authService;
        private readonly IVendorRegistrationProducerService _vendorRegistrationProducerService;
        private readonly IAddressService _addressService;

        public RegisterAsVendorCommandHandler(
            IAuthService authService,
            IVendorRegistrationProducerService vendorRegistrationProducerService,
            IAddressService addressService)
        {
            _authService = authService;
            _vendorRegistrationProducerService = vendorRegistrationProducerService;
            _addressService = addressService;
        }

        public async Task<VendorRegister> Handle(RegisterAsVendorCommand request, CancellationToken cancellationToken)
        {
            var registerVendor = await _authService.RegisterAsVendor(request.vendorRegister);
            //var registerVendor = new Tuple<VendorRegister, string>(request.vendorRegister, Guid.NewGuid().ToString());

            if (!string.IsNullOrEmpty(registerVendor.Item2))
                request.vendorRegister.UserId = registerVendor.Item2;

            if(request.ProcessQueue)
            {
                if (!registerVendor.Item1.Errors.Any() && !string.IsNullOrEmpty(registerVendor.Item2))
                {
                    var vendorRegistred = registerVendor.Item1;
                    var cityname = await _addressService.GetCityNameById(Convert.ToInt32(vendorRegistred.CityId));
                    var statename = await _addressService.GetStateNameById(Convert.ToInt32(vendorRegistred.StateId));
                    var arename = await _addressService.GetAreaNameById(Convert.ToInt32(vendorRegistred.AreaId));

                    var vendorBody = new VendorModel
                    {
                        VendorName = vendorRegistred.VendorName,
                        VendorDescription = vendorRegistred.VendorDescription,
                        Address = vendorRegistred.Address,
                        Email = vendorRegistred.Email,
                        Username = vendorRegistred.Username,
                        State = statename,
                        City = cityname,
                        Area = arename,
                        UserId = registerVendor.Item2
                    };
                    _vendorRegistrationProducerService.PublishVendorInformationToQueue(vendorBody);
                }
            }

            return request.vendorRegister;
        }
    }
}

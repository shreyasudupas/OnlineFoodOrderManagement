using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Register
{
    public class RegisterAsVendorCommand : IRequest<VendorRegisterModel>
    {
        public VendorRegisterModel vendorRegister { get; set; }
    }

    public class RegisterAsVendorCommandHandler : IRequestHandler<RegisterAsVendorCommand, VendorRegisterModel>
    {
        private readonly IAuthService _authService;
        public RegisterAsVendorCommandHandler(
            IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<VendorRegisterModel> Handle(RegisterAsVendorCommand request, CancellationToken cancellationToken)
        {
            var registerVendor = await _authService.RegisterAsVendor(request.vendorRegister);

            //if(request.ProcessQueue)
            //{
            //    if (!registerVendor.Item1.Errors.Any() && !string.IsNullOrEmpty(registerVendor.Item2))
            //    {
            //        var vendorRegistred = registerVendor.Item1;

            //        var vendorBody = new VendorModel
            //        {
            //            VendorName = vendorRegistred.VendorName,
            //            VendorDescription = vendorRegistred.VendorDescription,
            //            Email = vendorRegistred.Email,
            //            Username = vendorRegistred.Username,
            //            UserId = registerVendor.Item2
            //        };
            //        _vendorRegistrationProducerService.PublishVendorInformationToQueue(vendorBody);
            //    }
            //}

            return registerVendor;
        }
    }
}

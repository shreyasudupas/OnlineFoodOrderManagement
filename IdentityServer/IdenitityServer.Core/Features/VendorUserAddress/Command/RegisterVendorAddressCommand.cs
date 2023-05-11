using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorUserAddress.Command
{
    public class RegisterVendorAddressCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public UserAddress UserAddress { get; set; }
    }

    public class RegisterVendorAddressCommandHandler : IRequestHandler<RegisterVendorAddressCommand, bool>
    {
        private readonly IAuthService _authService;

        public RegisterVendorAddressCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(RegisterVendorAddressCommand request, CancellationToken cancellationToken)
        {
            return await _authService.AddVendorUserAddress(request.UserId, request.UserAddress);
        }
    }
}

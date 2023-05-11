using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorUserAddress.Queries
{
    public class GetVendorAddressByVendorIdQuery :  IRequest<UserAddress>
    {
        public string VendorId { get; set; }
    }

    public class GetVendorAddressByVendorIdQueryHandler : IRequestHandler<GetVendorAddressByVendorIdQuery, UserAddress>
    {
        private readonly IAuthService _authService;

        public GetVendorAddressByVendorIdQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<UserAddress> Handle(GetVendorAddressByVendorIdQuery request, CancellationToken cancellationToken)
        {
            return await _authService.GetVendorAddressByVendorId(request.VendorId);
        }
    }
}

using IdenitityServer.Core.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Vendor.Query.IsVendorEnabledQuery
{
    public class IsVendorEnabledQuery : IRequest<bool>
    {
        public string UserId { get; set; }
    }

    public class IsVendorEnabledQueryHandler : IRequestHandler<IsVendorEnabledQuery, bool>
    {
        private readonly IAuthService _authService;

        public IsVendorEnabledQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(IsVendorEnabledQuery request, CancellationToken cancellationToken)
        {
            return await _authService.GetVendorIsEnabled(request.UserId);
        }
    }
}

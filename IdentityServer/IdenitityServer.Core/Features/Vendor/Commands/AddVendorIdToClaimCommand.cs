using IdenitityServer.Core.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Vendor.Commands
{
    public class AddVendorIdToClaimCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string VendorId { get; set; }
    }

    public class AddVendorIdToClaimCommandHandler : IRequestHandler<AddVendorIdToClaimCommand, bool>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AddVendorIdToClaimCommandHandler> _logger;

        public AddVendorIdToClaimCommandHandler(IAuthService authService,
            ILogger<AddVendorIdToClaimCommandHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<bool> Handle(AddVendorIdToClaimCommand request, CancellationToken cancellationToken)
        {
            var isClaimAdded = await _authService.AddVendorClaim(request.UserId, request.VendorId);
            return isClaimAdded;
        }
    }
}

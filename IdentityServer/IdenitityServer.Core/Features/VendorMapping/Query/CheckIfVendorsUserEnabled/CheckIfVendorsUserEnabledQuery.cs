using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorMapping.Query.CheckIfVendorsUserEnabled
{
    public class CheckIfVendorsUserEnabledQuery : IRequest<VendorUserMappingEnableResponse>
    {
        public string UserId { get; set; }
        public string VendorId { get; set; }
    }

    public class CheckIfVendorsUserEnabledQueryHandler : IRequestHandler<CheckIfVendorsUserEnabledQuery, VendorUserMappingEnableResponse>
    {
        private readonly IUserService _userService;
        private readonly IVendorUserMappingService _vendorUserMappingService;
        private readonly ILogger<CheckIfVendorsUserEnabledQueryHandler> _logger;

        public CheckIfVendorsUserEnabledQueryHandler(IUserService userService,
            IVendorUserMappingService vendorUserMappingService,
            ILogger<CheckIfVendorsUserEnabledQueryHandler> logger)
        {
            _userService = userService;
            _vendorUserMappingService = vendorUserMappingService;
            _logger = logger;
        }

        public async Task<VendorUserMappingEnableResponse> Handle(CheckIfVendorsUserEnabledQuery request, CancellationToken cancellationToken)
        {
            VendorUserMappingEnableResponse response = new();
            response.VendorId = request.VendorId;

            var user = await _userService.GetUserInformationById(request.UserId);

            if(user is not null)
            {
                var userVendorMapping = await _vendorUserMappingService.GetVendorUserMapping(request.UserId, request.VendorId);
                if (userVendorMapping is not null)
                {
                    response.IsEnabled = user.Enabled;
                    response.IsVendorPresent = true;
                    return response;
                }
                else
                {
                    response.IsVendorPresent = false;
                    response.IsEnabled = false;
                    _logger.LogError("User Vendor not present with UserId {0} and VendorID {1}", request.UserId,request.VendorId);
                    return response;
                }

            }
            else
            {
                _logger.LogError("User not present with UserId {0}",request.UserId);
                return null;
            }

        }
    }
}

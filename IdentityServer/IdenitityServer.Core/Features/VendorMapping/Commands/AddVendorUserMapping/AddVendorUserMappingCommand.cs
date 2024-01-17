using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorMapping.Commands.AddVendorUserMapping
{
    public class AddVendorUserMappingCommand : IRequest<VendorMappingResponse>
    {
        public VendorMappingResponse NewVendorUserMapping { get; set; }
    }

    public class AddVendorUserMappingCommandHandler : IRequestHandler<AddVendorUserMappingCommand, VendorMappingResponse>
    {
        private readonly IVendorUserMappingService _vendorUserMappingService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ILogger<AddVendorUserMappingCommandHandler> _logger;

        public AddVendorUserMappingCommandHandler(IVendorUserMappingService vendorUserMappingService
            , IMapper mapper,
            IAuthService authService,
            ILogger<AddVendorUserMappingCommandHandler> logger)
        {
            _vendorUserMappingService = vendorUserMappingService;
            _mapper = mapper;
            _authService = authService;
            _logger = logger;
        }

        public async Task<VendorMappingResponse> Handle(AddVendorUserMappingCommand request, CancellationToken cancellationToken)
        {
            var mapToModel = _mapper.Map<VendorUserIdMapping>(request.NewVendorUserMapping);
            var result = await _vendorUserMappingService.AddVendorUserIdMapping(mapToModel);

            if(!string.IsNullOrEmpty(request.NewVendorUserMapping.VendorId))
            {
                var vendorClaim = await _authService.AddVendorClaim(mapToModel.UserId,mapToModel.VendorId);
                if (vendorClaim)
                {
                    _logger.LogInformation("Vendor Claim Added for Vendor {0}",request.NewVendorUserMapping.VendorId);
                }
                else
                {
                    _logger.LogError("Error Occured in Saving the Vendor Claim for VendorId {0}", request.NewVendorUserMapping.VendorId);
                }
                
            }
            var mapToResposne = _mapper.Map<VendorMappingResponse>(result);
            return mapToResposne;
        }
    }
}

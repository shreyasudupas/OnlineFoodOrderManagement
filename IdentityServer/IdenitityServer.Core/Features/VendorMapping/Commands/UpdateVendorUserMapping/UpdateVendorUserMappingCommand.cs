using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorMapping.Commands.UpdateVendorUserMapping
{
    public class UpdateVendorUserMappingCommand : IRequest<VendorMappingResponse>
    {
        public VendorMappingResponse UpdateVendorUserMapping { get; set; }
    }

    public class UpdateVendorUserMappingCommandHandler : IRequestHandler<UpdateVendorUserMappingCommand, VendorMappingResponse>
    {
        private readonly IVendorUserMappingService _vendorUserMappingService;
        private readonly IMapper _mapper;
        private readonly IUtilsService _utilsService;

        public UpdateVendorUserMappingCommandHandler(IVendorUserMappingService vendorUserMappingService,
            IMapper mapper,
            IUtilsService utilsService)
        {
            _vendorUserMappingService = vendorUserMappingService;
            _mapper = mapper;
            _utilsService = utilsService;
        }

        public async Task<VendorMappingResponse> Handle(UpdateVendorUserMappingCommand request, CancellationToken cancellationToken)
        {
            var mapToModel = _mapper.Map<VendorUserIdMapping>(request.UpdateVendorUserMapping);
            var result = await _vendorUserMappingService.UpdateVendorUserIdMapping(mapToModel,request.UpdateVendorUserMapping.Enabled);
            var mapToResponse = _mapper.Map<VendorMappingResponse>(result);

            if (result is not null)
            {
                mapToResponse.Enabled = request.UpdateVendorUserMapping.Enabled;
            }

            return mapToResponse;
        }
    }
}

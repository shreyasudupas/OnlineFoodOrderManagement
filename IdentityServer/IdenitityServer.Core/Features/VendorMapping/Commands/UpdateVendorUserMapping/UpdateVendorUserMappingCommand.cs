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

        public UpdateVendorUserMappingCommandHandler(IVendorUserMappingService vendorUserMappingService
            , IMapper mapper)
        {
            _vendorUserMappingService = vendorUserMappingService;
            _mapper = mapper;
        }

        public async Task<VendorMappingResponse> Handle(UpdateVendorUserMappingCommand request, CancellationToken cancellationToken)
        {
            var mapToModel = _mapper.Map<VendorUserIdMapping>(request.UpdateVendorUserMapping);
            var result = await _vendorUserMappingService.UpdateVendorUserIdMapping(mapToModel);
            var mapToResposne = _mapper.Map<VendorMappingResponse>(result);
            return mapToResposne;
        }
    }
}

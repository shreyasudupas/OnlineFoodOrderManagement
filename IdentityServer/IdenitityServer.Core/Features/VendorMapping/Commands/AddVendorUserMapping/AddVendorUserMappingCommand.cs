using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using MediatR;
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

        public AddVendorUserMappingCommandHandler(IVendorUserMappingService vendorUserMappingService
            , IMapper mapper)
        {
            _vendorUserMappingService = vendorUserMappingService;
            _mapper = mapper;
        }

        public async Task<VendorMappingResponse> Handle(AddVendorUserMappingCommand request, CancellationToken cancellationToken)
        {
            var mapToModel = _mapper.Map<VendorUserIdMapping>(request.NewVendorUserMapping);
            var result = await _vendorUserMappingService.AddVendorUserIdMapping(mapToModel);
            var mapToResposne = _mapper.Map<VendorMappingResponse>(result);
            return mapToResposne;
        }
    }
}

using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorMapping.Query.GetVendorUserBasedOnEmailId
{
    public class GetVendorUserMappingBasedOnEmailIdQuery : IRequest<VendorMappingResponse>
    {
        public string EmailId { get; set; }
    }

    public class GetVendorUserMappingBasedOnEmailIdQueryHandler : IRequestHandler<GetVendorUserMappingBasedOnEmailIdQuery, VendorMappingResponse>
    {
        private readonly IVendorUserMappingService _vendorUserMappingService;
        private readonly IMapper _mapper;

        public GetVendorUserMappingBasedOnEmailIdQueryHandler(IVendorUserMappingService vendorUserMappingService,
            IMapper mapper)
        {
            _vendorUserMappingService = vendorUserMappingService;
            _mapper = mapper;
        }

        public async Task<VendorMappingResponse> Handle(GetVendorUserMappingBasedOnEmailIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _vendorUserMappingService.GetVendorUserMappingBasedOnEmailId(request.EmailId);
            var mapResult = _mapper.Map<VendorMappingResponse>(result);
            return mapResult;
        }
    }
}

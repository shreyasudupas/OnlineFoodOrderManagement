using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorMapping.Query.GetAllVendorUserMapping
{
    public class GetAllVendorUserMappingQuery : IRequest<List<VendorMappingResponse>>
    {
        public string VendorId { get; set; }
    }

    public class GetAllVendorUserMappingQueryHandler : IRequestHandler<GetAllVendorUserMappingQuery, List<VendorMappingResponse>>
    {
        private readonly IVendorUserMappingService _vendorUserMappingService;

        public GetAllVendorUserMappingQueryHandler(
            IVendorUserMappingService vendorUserMappingService
            )
        {
            _vendorUserMappingService = vendorUserMappingService;
            
        }

        public async Task<List<VendorMappingResponse>> Handle(GetAllVendorUserMappingQuery request, CancellationToken cancellationToken)
        {
            var userMappingResult = await _vendorUserMappingService.GetVendorUserMapping(request.VendorId);
            return userMappingResult;
        }
    }
}

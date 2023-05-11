using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility.VendorIdMapping
{
    public class GetAllVendorIdMappingQuery : IRequest<List<VendorIdMappingResponse>>
    {
        public string? VendorId { get; set; }
    }

    public class GetAllVendorIdMappingQueryHandler : IRequestHandler<GetAllVendorIdMappingQuery, List<VendorIdMappingResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllVendorIdMappingQueryHandler(IMapper mapper,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<VendorIdMappingResponse>> Handle(GetAllVendorIdMappingQuery request, CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllVendorUserIdMapping(request.VendorId);
            var mapToDto = _mapper.Map<List<VendorIdMappingResponse>>(result);
            return mapToDto;
        }
    }
}

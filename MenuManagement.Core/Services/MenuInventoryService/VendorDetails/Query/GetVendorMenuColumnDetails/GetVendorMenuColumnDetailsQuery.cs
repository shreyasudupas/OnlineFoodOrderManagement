using AutoMapper;
using MediatR;
using MenuManagment.Domain.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorMenuColumnDetails
{
    public class GetVendorMenuColumnDetailsQuery : IRequest<List<VendorColumnDetailDto>>
    {
        public string VendorId { get; set; }
    }

    public class GetVendorMenuColumnDetailsQueryHandler : IRequestHandler<GetVendorMenuColumnDetailsQuery, List<VendorColumnDetailDto>>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetVendorMenuColumnDetailsQueryHandler(IMenuRepository menuRepository,IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorColumnDetailDto>> Handle(GetVendorMenuColumnDetailsQuery request, CancellationToken cancellationToken)
        {
            var vendorMenuColumnResult = await _menuRepository.ListVendorMenuColumnDetails(request.VendorId);

            var vendorMenuColumnModelMapping = _mapper.Map<List<VendorColumnDetailDto>>(vendorMenuColumnResult);

            return vendorMenuColumnModelMapping;
        }
    }
}

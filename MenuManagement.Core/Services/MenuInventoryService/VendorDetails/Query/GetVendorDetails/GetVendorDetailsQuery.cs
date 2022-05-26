using AutoMapper;
using MediatR;
using MenuManagment.Domain.Mongo.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorDetails
{
    public class GetVendorDetailsQuery : IRequest<List<VendorDetailsResponseDto>>
    {
        public string Locality { get; set; }
    }

    public class GetVendorDetailsQueryHandler : IRequestHandler<GetVendorDetailsQuery, List<VendorDetailsResponseDto>>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetVendorDetailsQueryHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorDetailsResponseDto>> Handle(GetVendorDetailsQuery request, CancellationToken cancellationToken)
        {
            var vendorDetail = await _menuRepository.ListAllVendorDetails(request.Locality);

            //var resultConvertToDto = _mapper.Map<List<VendorDetailsResponseDto>>(vendorDetail);
            var resultConvertToDto = vendorDetail.Select(x=> new VendorDetailsResponseDto
            {
                VendorId = x.VendorId,
                Description = x.Description,
                Location = x.Location,
                Rating = x.Rating,
                VendorName = x.VendorName
            }).ToList();

            return resultConvertToDto;
        }
    }
}

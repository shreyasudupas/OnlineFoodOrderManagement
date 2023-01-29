using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorList
{
    public class VendorListQuery : IRequest<List<VendorDto>>
    {
    }

    public class VendorListQueryHandler : IRequestHandler<VendorListQuery, List<VendorDto>>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;
        public VendorListQueryHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        public async Task<List<VendorDto>> Handle(VendorListQuery request, CancellationToken cancellationToken)
        {
            var result = await _vendorRepository.GetAllVendorDocuments();
            if (result.Count > 0)
            {
                var mapToDto = _mapper.Map<List<VendorDto>>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}

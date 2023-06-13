using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.Vendor.VendorDetails.Commands.AddVendors
{
    public class AddVendorsCommand : IRequest<List<VendorDto>>
    {
        public List<VendorDto> VendorsInput { get; set; }
    }

    public class AddVendorCommandHandler : IRequestHandler<AddVendorsCommand, List<VendorDto>>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public AddVendorCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorDto>> Handle(AddVendorsCommand request, CancellationToken cancellationToken)
        {
            var result = await _vendorRepository.AddVendorDocuments(request.VendorsInput);
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

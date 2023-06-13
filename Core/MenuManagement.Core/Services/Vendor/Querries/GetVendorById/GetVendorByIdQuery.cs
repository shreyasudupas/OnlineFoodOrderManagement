using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.Vendor.Querries.GetVendorById
{
    public class GetVendorByIdQuery : IRequest<VendorDto>
    {
        public string VendorId { get; set; }
    }

    public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorDto>
    {
        private readonly IVendorRepository vendorRepository;
        private readonly IMapper _mapper;

        public GetVendorByIdQueryHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            this.vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<VendorDto> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await vendorRepository.GetVendorDocument(request.VendorId);
            if (result != null)
            {
                var mapToDto = _mapper.Map<VendorDto>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}

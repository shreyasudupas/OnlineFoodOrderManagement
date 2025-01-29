using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.Vendors.Commands.UpdateVendorDetails
{
    public class UpdateVendorDetailsCommand : IRequest<VendorDto>
    {
        public VendorDto VendorDetail { get; set; }
    }

    public class UpdateVendorDetailsCommandHandler : IRequestHandler<UpdateVendorDetailsCommand, VendorDto>
    {
        private readonly IVendorRepository vendorRepository;
        private readonly IMapper _mapper;

        public UpdateVendorDetailsCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            this.vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<VendorDto> Handle(UpdateVendorDetailsCommand request, CancellationToken cancellationToken)
        {
            var mapToVendorModel = _mapper.Map<MenuManagment.Mongo.Domain.Mongo.Entities.Vendor>(request.VendorDetail);
            var result = await vendorRepository.UpdateVendorDocument(mapToVendorModel);
            if (result != null)
            {
                var mapToModel = _mapper.Map<VendorDto>(result);
                return mapToModel;
            }
            else
                return null;
        }
    }
}

using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.Vendors.Commands.AddVendorDetail
{
    public class AddVendorDetailCommand : IRequest<VendorDto>
    {
        public VendorDto VendorDetail { get; set; }
    }

    public class AddvendorDetailCommandHandler : IRequestHandler<AddVendorDetailCommand, VendorDto>
    {
        private readonly IVendorRepository vendorRepository;
        private readonly IMapper _mapper;

        public AddvendorDetailCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            this.vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<VendorDto> Handle(AddVendorDetailCommand request, CancellationToken cancellationToken)
        {
            var mapToVendorModel = _mapper.Map<MenuManagment.Mongo.Domain.Mongo.Entities.Vendor>(request.VendorDetail);
            var result = await vendorRepository.AddVendorDocument(mapToVendorModel);
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

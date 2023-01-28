using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.Vendor.UpdateVendorDetails
{
    public class UpdateVendorDetailsCommand : IRequest<VendorDto>
    {
        public VendorDto VendorDetail { get; set; }
    }

    public class UpdateVendorDetailsCommandHandler : IRequestHandler<UpdateVendorDetailsCommand, VendorDto>
    {
        private readonly IVendorRepository vendorRepository;

        public UpdateVendorDetailsCommandHandler(IVendorRepository vendorRepository)
        {
            this.vendorRepository = vendorRepository;
        }

        public async Task<VendorDto> Handle(UpdateVendorDetailsCommand request, CancellationToken cancellationToken)
        {
            return await vendorRepository.UpdateVendorDocument(request.VendorDetail);
        }
    }
}

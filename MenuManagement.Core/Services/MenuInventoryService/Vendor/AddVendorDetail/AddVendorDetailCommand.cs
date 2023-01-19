using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.AddVendorDetail
{
    public class AddVendorDetailCommand : IRequest<VendorDto>
    {
        public VendorDto VendorDetail { get; set; }
    }

    public class AddvendorDetailCommandHandler : IRequestHandler<AddVendorDetailCommand, VendorDto>
    {
        private readonly IVendorRepository vendorRepository;

        public AddvendorDetailCommandHandler(IVendorRepository vendorRepository)
        {
            this.vendorRepository = vendorRepository;
        }

        public async Task<VendorDto> Handle(AddVendorDetailCommand request, CancellationToken cancellationToken)
        {
            return await vendorRepository.AddVendorDocument(request.VendorDetail);
        }
    }
}

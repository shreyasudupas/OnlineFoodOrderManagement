using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.GetVendorById
{
    public class GetVendorByIdQuery : IRequest<VendorDto>
    {
        public string VendorId { get; set; }
    }

    public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorDto>
    {
        private readonly IVendorRepository vendorRepository;

        public GetVendorByIdQueryHandler(IVendorRepository vendorRepository)
        {
            this.vendorRepository = vendorRepository;
        }

        public async Task<VendorDto> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            return await vendorRepository.GetVendorDocument(request.VendorId);
        }
    }
}

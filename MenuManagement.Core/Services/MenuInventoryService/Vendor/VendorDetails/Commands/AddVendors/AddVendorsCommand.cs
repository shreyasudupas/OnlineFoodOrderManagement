using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Commands.AddVendors
{
    public class AddVendorsCommand : IRequest<List<VendorDto>>
    {
        public List<VendorDto> VendorsInput { get; set; }
    }

    public class AddVendorCommandHandler : IRequestHandler<AddVendorsCommand, List<VendorDto>>
    {
        private readonly IVendorRepository _vendorRepository;

        public AddVendorCommandHandler(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<List<VendorDto>> Handle(AddVendorsCommand request, CancellationToken cancellationToken)
        {
            return await _vendorRepository.AddVendorDocuments(request.VendorsInput);
        }
    }
}

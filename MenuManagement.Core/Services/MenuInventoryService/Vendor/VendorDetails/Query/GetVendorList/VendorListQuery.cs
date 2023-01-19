using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorList
{
    public class VendorListQuery : IRequest<List<VendorDto>>
    {
    }

    public class VendorListQueryHandler : IRequestHandler<VendorListQuery, List<VendorDto>>
    {
        private readonly IVendorRepository _vendorRepository;
        public VendorListQueryHandler(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }
        public Task<List<VendorDto>> Handle(VendorListQuery request, CancellationToken cancellationToken)
        {
            return _vendorRepository.GetAllVendorDocuments();
        }
    }
}

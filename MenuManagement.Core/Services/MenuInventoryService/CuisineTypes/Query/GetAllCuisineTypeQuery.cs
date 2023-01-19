using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.CuisineTypes.Query
{
    public class GetAllCuisineTypeQuery : IRequest<List<VendorCuisineDto>>
    {
        public bool Active { get; set; }
    }

    public class GetAllCuisineTypeQueryHandler : IRequestHandler<GetAllCuisineTypeQuery, List<VendorCuisineDto>>
    {
        private readonly IVendorCuisineTypeRepository vendorCuisineTypeRepository;

        public GetAllCuisineTypeQueryHandler(IVendorCuisineTypeRepository vendorCuisineTypeRepository)
        {
            this.vendorCuisineTypeRepository = vendorCuisineTypeRepository;
        }

        public async Task<List<VendorCuisineDto>> Handle(GetAllCuisineTypeQuery request, CancellationToken cancellationToken)
        {
            return await vendorCuisineTypeRepository.GetListVendorCuisineType(request.Active);
        }
    }
}

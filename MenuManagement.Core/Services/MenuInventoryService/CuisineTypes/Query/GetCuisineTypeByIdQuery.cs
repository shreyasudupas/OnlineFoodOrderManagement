using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.CuisineTypes.Query
{
    public class GetCuisineTypeByIdQuery : IRequest<VendorCuisineDto>
    {
        public string Id { get; set; }
    }

    public class GetCuisineTypeByIdQueryHandler : IRequestHandler<GetCuisineTypeByIdQuery, VendorCuisineDto>
    {
        private readonly IVendorCuisineTypeRepository vendorCuisineTypeRepository;

        public GetCuisineTypeByIdQueryHandler(IVendorCuisineTypeRepository vendorCuisineTypeRepository)
        {
            this.vendorCuisineTypeRepository = vendorCuisineTypeRepository;
        }

        public async Task<VendorCuisineDto> Handle(GetCuisineTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await vendorCuisineTypeRepository.GetVendorCuisineTypeById(request.Id);
        }
    }
}

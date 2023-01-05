using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.FoodTypes.Query
{
    public class GetFoodTypeByIdQuery : IRequest<VendorFoodTypeDto>
    {
        public string Id { get; set; }
    }

    public class GetFoodTypeByIdQueryHandler : IRequestHandler<GetFoodTypeByIdQuery, VendorFoodTypeDto>
    {
        private readonly IVendorFoodTypeRepository vendorFoodTypeRepository;

        public GetFoodTypeByIdQueryHandler(IVendorFoodTypeRepository vendorFoodTypeRepository)
        {
            this.vendorFoodTypeRepository = vendorFoodTypeRepository;
        }

        public async Task<VendorFoodTypeDto> Handle(GetFoodTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await vendorFoodTypeRepository.GetVendorFoodTypeById(request.Id);
        }
    }
}

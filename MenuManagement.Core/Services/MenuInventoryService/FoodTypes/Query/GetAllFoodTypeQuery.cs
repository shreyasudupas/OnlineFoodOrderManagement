using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.FoodTypes.Query
{
    public class GetAllFoodTypeQuery : IRequest<List<VendorFoodTypeDto>>
    {
        public bool Active { get; set; }
    }

    public class GetAllFoodTypeQueryHandler : IRequestHandler<GetAllFoodTypeQuery, List<VendorFoodTypeDto>>
    {
        private readonly IVendorFoodTypeRepository vendorFoodTypeRepository;

        public GetAllFoodTypeQueryHandler(IVendorFoodTypeRepository vendorFoodTypeRepository)
        {
            this.vendorFoodTypeRepository = vendorFoodTypeRepository;
        }

        public async Task<List<VendorFoodTypeDto>> Handle(GetAllFoodTypeQuery request, CancellationToken cancellationToken)
        {
            return await vendorFoodTypeRepository.GetListVendorFoodType(request.Active);
        }
    }
}

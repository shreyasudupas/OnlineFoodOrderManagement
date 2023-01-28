using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.FoodTypes.Command
{
    public class AddFoodTypeCommand : IRequest<VendorFoodTypeDto>
    {
        public VendorFoodTypeDto FoodType { get; set; }
    }

    public class AddFoodTypeCommandHandler : IRequestHandler<AddFoodTypeCommand, VendorFoodTypeDto>
    {
        private readonly IVendorFoodTypeRepository vendorFoodTypeRepository;

        public AddFoodTypeCommandHandler(IVendorFoodTypeRepository vendorFoodTypeRepository)
        {
            this.vendorFoodTypeRepository = vendorFoodTypeRepository;
        }

        public async Task<VendorFoodTypeDto> Handle(AddFoodTypeCommand request, CancellationToken cancellationToken)
        {
            return await vendorFoodTypeRepository.AddVendorFoodType(request.FoodType);
        }
    }
}

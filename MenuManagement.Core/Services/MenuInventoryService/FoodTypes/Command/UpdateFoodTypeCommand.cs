using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.FoodTypes.Command
{
    public class UpdateFoodTypeCommand : IRequest<VendorFoodTypeDto>
    {
        public VendorFoodTypeDto UpdatedFoodType { get; set; }
    }
    public class UpdatedFoodTypeCommandHandler : IRequestHandler<UpdateFoodTypeCommand, VendorFoodTypeDto>
    {
        private readonly IVendorFoodTypeRepository vendorFoodTypeRepository;

        public UpdatedFoodTypeCommandHandler(IVendorFoodTypeRepository vendorFoodTypeRepository)
        {
            this.vendorFoodTypeRepository = vendorFoodTypeRepository;
        }

        public async Task<VendorFoodTypeDto> Handle(UpdateFoodTypeCommand request, CancellationToken cancellationToken)
        {
            return await vendorFoodTypeRepository.UpdateFoodTypeDocument(request.UpdatedFoodType);
        }
    }
}

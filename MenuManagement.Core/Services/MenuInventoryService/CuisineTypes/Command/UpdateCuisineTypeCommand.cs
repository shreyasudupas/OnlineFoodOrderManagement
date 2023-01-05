using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.CuisineTypes.Command
{
    public class UpdateCuisineTypeCommand : IRequest<VendorCuisineDto>
    {
        public VendorCuisineDto UpdatedCuisineType { get; set; }
    }
    public class UpdateCuisineTypeCommandHandler : IRequestHandler<UpdateCuisineTypeCommand, VendorCuisineDto>
    {
        private readonly IVendorCuisineTypeRepository vendorCuisineTypeRepository;

        public UpdateCuisineTypeCommandHandler(IVendorCuisineTypeRepository vendorCuisineTypeRepository)
        {
            this.vendorCuisineTypeRepository = vendorCuisineTypeRepository;
        }

        public async Task<VendorCuisineDto> Handle(UpdateCuisineTypeCommand request, CancellationToken cancellationToken)
        {
            return await vendorCuisineTypeRepository.UpdateFoodTypeDocument(request.UpdatedCuisineType);
        }
    }
}

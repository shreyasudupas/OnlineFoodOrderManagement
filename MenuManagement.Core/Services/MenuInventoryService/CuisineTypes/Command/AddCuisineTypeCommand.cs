using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.CuisineTypes.Command
{
    public class AddCuisineTypeCommand : IRequest<VendorCuisineDto>
    {
        public VendorCuisineDto CuisineType { get; set; }
    }

    public class AddCuisineTypeCommandHandler : IRequestHandler<AddCuisineTypeCommand, VendorCuisineDto>
    {
        private readonly IVendorCuisineTypeRepository vendorCuisineTypeRepository;

        public AddCuisineTypeCommandHandler(IVendorCuisineTypeRepository vendorCuisineTypeRepository)
        {
            this.vendorCuisineTypeRepository = vendorCuisineTypeRepository;
        }

        public async Task<VendorCuisineDto> Handle(AddCuisineTypeCommand request, CancellationToken cancellationToken)
        {
            return await vendorCuisineTypeRepository.AddVendorCuisineType(request.CuisineType);
        }
    }
}

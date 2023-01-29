using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.FoodTypes.Command
{
    public class AddFoodTypeCommand : IRequest<VendorFoodTypeDto>
    {
        public VendorFoodTypeDto FoodType { get; set; }
    }

    public class AddFoodTypeCommandHandler : IRequestHandler<AddFoodTypeCommand, VendorFoodTypeDto>
    {
        private readonly IVendorFoodTypeRepository vendorFoodTypeRepository;
        private readonly IMapper _mapper;

        public AddFoodTypeCommandHandler(IVendorFoodTypeRepository vendorFoodTypeRepository, IMapper mapper)
        {
            this.vendorFoodTypeRepository = vendorFoodTypeRepository;
            _mapper = mapper;
        }

        public async Task<VendorFoodTypeDto> Handle(AddFoodTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await vendorFoodTypeRepository.AddVendorFoodType(request.FoodType);
            if (result != null)
            {
                var mapToDto = _mapper.Map<VendorFoodTypeDto>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}

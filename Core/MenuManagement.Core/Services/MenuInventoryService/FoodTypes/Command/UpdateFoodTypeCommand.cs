using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.FoodTypes.Command
{
    public class UpdateFoodTypeCommand : IRequest<VendorFoodTypeDto>
    {
        public VendorFoodTypeDto UpdatedFoodType { get; set; }
    }
    public class UpdatedFoodTypeCommandHandler : IRequestHandler<UpdateFoodTypeCommand, VendorFoodTypeDto>
    {
        private readonly IVendorFoodTypeRepository vendorFoodTypeRepository;
        private readonly IMapper _mapper;

        public UpdatedFoodTypeCommandHandler(IVendorFoodTypeRepository vendorFoodTypeRepository, IMapper mapper)
        {
            this.vendorFoodTypeRepository = vendorFoodTypeRepository;
            _mapper = mapper;
        }

        public async Task<VendorFoodTypeDto> Handle(UpdateFoodTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await vendorFoodTypeRepository.UpdateFoodTypeDocument(request.UpdatedFoodType);
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

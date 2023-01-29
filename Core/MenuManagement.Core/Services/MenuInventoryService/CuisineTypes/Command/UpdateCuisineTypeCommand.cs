using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.CuisineTypes.Command
{
    public class UpdateCuisineTypeCommand : IRequest<VendorCuisineDto>
    {
        public VendorCuisineDto UpdatedCuisineType { get; set; }
    }
    public class UpdateCuisineTypeCommandHandler : IRequestHandler<UpdateCuisineTypeCommand, VendorCuisineDto>
    {
        private readonly IVendorCuisineTypeRepository vendorCuisineTypeRepository;
        private readonly IMapper _mapper;

        public UpdateCuisineTypeCommandHandler(IVendorCuisineTypeRepository vendorCuisineTypeRepository, IMapper mapper)
        {
            this.vendorCuisineTypeRepository = vendorCuisineTypeRepository;
            _mapper = mapper;
        }

        public async Task<VendorCuisineDto> Handle(UpdateCuisineTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await vendorCuisineTypeRepository.UpdateFoodTypeDocument(request.UpdatedCuisineType);
            if (result != null)
            {
                var mapTomodel = _mapper.Map<VendorCuisineDto>(result);
                return mapTomodel;
            }
            else
                return null;
        }
    }
}

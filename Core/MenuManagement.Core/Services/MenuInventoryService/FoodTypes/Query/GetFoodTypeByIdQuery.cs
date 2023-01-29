using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.FoodTypes.Query
{
    public class GetFoodTypeByIdQuery : IRequest<VendorFoodTypeDto>
    {
        public string Id { get; set; }
    }

    public class GetFoodTypeByIdQueryHandler : IRequestHandler<GetFoodTypeByIdQuery, VendorFoodTypeDto>
    {
        private readonly IVendorFoodTypeRepository vendorFoodTypeRepository;
        private readonly IMapper _mapper;

        public GetFoodTypeByIdQueryHandler(IVendorFoodTypeRepository vendorFoodTypeRepository, IMapper mapper)
        {
            this.vendorFoodTypeRepository = vendorFoodTypeRepository;
            _mapper = mapper;
        }

        public async Task<VendorFoodTypeDto> Handle(GetFoodTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await vendorFoodTypeRepository.GetVendorFoodTypeById(request.Id);
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

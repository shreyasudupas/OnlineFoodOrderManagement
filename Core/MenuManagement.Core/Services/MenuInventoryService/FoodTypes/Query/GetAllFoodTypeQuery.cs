using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.FoodTypes.Query
{
    public class GetAllFoodTypeQuery : IRequest<List<VendorFoodTypeDto>>
    {
        public bool Active { get; set; }
    }

    public class GetAllFoodTypeQueryHandler : IRequestHandler<GetAllFoodTypeQuery, List<VendorFoodTypeDto>>
    {
        private readonly IVendorFoodTypeRepository vendorFoodTypeRepository;
        private readonly IMapper _mapper;

        public GetAllFoodTypeQueryHandler(IVendorFoodTypeRepository vendorFoodTypeRepository, IMapper mapper)
        {
            this.vendorFoodTypeRepository = vendorFoodTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorFoodTypeDto>> Handle(GetAllFoodTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await vendorFoodTypeRepository.GetListVendorFoodType(request.Active);
            if (result.Count > 0)
            {
                var mapToDto = _mapper.Map<List<VendorFoodTypeDto>>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}

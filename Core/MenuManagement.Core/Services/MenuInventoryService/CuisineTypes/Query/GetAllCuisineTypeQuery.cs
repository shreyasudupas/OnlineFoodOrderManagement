using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.CuisineTypes.Query
{
    public class GetAllCuisineTypeQuery : IRequest<List<VendorCuisineDto>>
    {
        public bool Active { get; set; }
    }

    public class GetAllCuisineTypeQueryHandler : IRequestHandler<GetAllCuisineTypeQuery, List<VendorCuisineDto>>
    {
        private readonly IVendorCuisineTypeRepository vendorCuisineTypeRepository;
        private readonly IMapper _mapper;

        public GetAllCuisineTypeQueryHandler(IVendorCuisineTypeRepository vendorCuisineTypeRepository, IMapper mapper)
        {
            this.vendorCuisineTypeRepository = vendorCuisineTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorCuisineDto>> Handle(GetAllCuisineTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await vendorCuisineTypeRepository.GetListVendorCuisineType(request.Active);
            if (result.Count > 0)
            {
                var mapToDto = _mapper.Map<List<VendorCuisineDto>>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}

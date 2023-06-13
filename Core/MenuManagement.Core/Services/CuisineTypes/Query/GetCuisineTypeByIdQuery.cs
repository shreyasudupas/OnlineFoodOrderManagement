using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.CuisineTypes.Query
{
    public class GetCuisineTypeByIdQuery : IRequest<VendorCuisineDto>
    {
        public string Id { get; set; }
    }

    public class GetCuisineTypeByIdQueryHandler : IRequestHandler<GetCuisineTypeByIdQuery, VendorCuisineDto>
    {
        private readonly IVendorCuisineTypeRepository vendorCuisineTypeRepository;
        private readonly IMapper _mapper;

        public GetCuisineTypeByIdQueryHandler(IVendorCuisineTypeRepository vendorCuisineTypeRepository, IMapper mapper)
        {
            this.vendorCuisineTypeRepository = vendorCuisineTypeRepository;
            _mapper = mapper;
        }

        public async Task<VendorCuisineDto> Handle(GetCuisineTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await vendorCuisineTypeRepository.GetVendorCuisineTypeById(request.Id);
            if (result != null)
            {
                var mapToDto = _mapper.Map<VendorCuisineDto>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}

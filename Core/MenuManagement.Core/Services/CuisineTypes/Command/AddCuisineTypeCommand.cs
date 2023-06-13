using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.CuisineTypes.Command
{
    public class AddCuisineTypeCommand : IRequest<VendorCuisineDto>
    {
        public VendorCuisineDto CuisineType { get; set; }
    }

    public class AddCuisineTypeCommandHandler : IRequestHandler<AddCuisineTypeCommand, VendorCuisineDto>
    {
        private readonly IVendorCuisineTypeRepository vendorCuisineTypeRepository;
        private readonly IMapper _mapper;

        public AddCuisineTypeCommandHandler(IVendorCuisineTypeRepository vendorCuisineTypeRepository, IMapper mapper)
        {
            this.vendorCuisineTypeRepository = vendorCuisineTypeRepository;
            _mapper = mapper;
        }

        public async Task<VendorCuisineDto> Handle(AddCuisineTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await vendorCuisineTypeRepository.AddVendorCuisineType(request.CuisineType);
            if (result != null)
            {
                var mapToDto = _mapper.Map<VendorCuisineDto>(result);
                return mapToDto;
            }
            else
            {
                return null;
            }
        }
    }
}

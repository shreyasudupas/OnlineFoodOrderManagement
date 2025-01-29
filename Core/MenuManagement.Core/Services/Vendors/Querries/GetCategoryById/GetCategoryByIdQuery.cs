using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.Vendor.Querries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto>
    {
        public string Id { get; set; }
        public string VendorId { get; set; }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IVendorRepository vendorRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            this.vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await vendorRepository.GetCategoryById(request.Id, request.VendorId);
            if (result != null)
            {
                var mapToDto = _mapper.Map<CategoryDto>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}

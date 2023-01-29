using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.AddCategory
{
    public class AddCategoryCommand : IRequest<CategoryDto>
    {
        public string VendorId { get; set; }
        public CategoryDto newCategory { get; set; }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryDto>
    {
        private readonly IVendorRepository vendorRepository;
        private readonly IMapper _mapper;

        public AddCategoryCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            this.vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = await vendorRepository.AddCategoryToVendor(request.VendorId,request.newCategory);
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

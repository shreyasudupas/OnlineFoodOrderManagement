using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.AddCategory
{
    public class AddCategoryCommand : IRequest<CategoryDto>
    {
        public string VendorId { get; set; }
        public CategoryDto newCategory { get; set; }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryDto>
    {
        private readonly IVendorRepository vendorRepository;

        public AddCategoryCommandHandler(IVendorRepository vendorRepository)
        {
            this.vendorRepository = vendorRepository;
        }

        public async Task<CategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            return await vendorRepository.AddCategoryToVendor(request.VendorId,request.newCategory);
        }
    }
}

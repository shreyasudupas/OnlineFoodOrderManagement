using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.Vendor.Commands
{
    public class UpdateCategoryItemCommand : IRequest<CategoryDto>
    {
        public string VendorId { get; set; }
        public CategoryDto Category { get; set; }
    }

    public class UpdateCategoryItemCommandHandler : IRequestHandler<UpdateCategoryItemCommand, CategoryDto>
    {
        private readonly IVendorRepository vendorRepository;

        public UpdateCategoryItemCommandHandler(IVendorRepository vendorRepository)
        {
            this.vendorRepository = vendorRepository;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryItemCommand request, CancellationToken cancellationToken)
        {
            return await vendorRepository.UpdateVendorCategoryDocument(request.VendorId,request.Category);
        }
    }
}

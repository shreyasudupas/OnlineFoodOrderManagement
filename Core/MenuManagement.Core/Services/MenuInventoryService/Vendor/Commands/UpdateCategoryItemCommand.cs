using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.Vendor.Commands
{
    public class UpdateCategoryItemCommand : IRequest<CategoryDto>
    {
        public string VendorId { get; set; }
        public CategoryDto Category { get; set; }
    }

    public class UpdateCategoryItemCommandHandler : IRequestHandler<UpdateCategoryItemCommand, CategoryDto>
    {
        private readonly IVendorRepository vendorRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryItemCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            this.vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryItemCommand request, CancellationToken cancellationToken)
        {
            var result = await vendorRepository.UpdateVendorCategoryDocument(request.VendorId,request.Category);
            if (result != null)
            {
                var mapToModel = _mapper.Map<CategoryDto>(result);
                return mapToModel;
            }
            else
                return null;
        }
    }
}

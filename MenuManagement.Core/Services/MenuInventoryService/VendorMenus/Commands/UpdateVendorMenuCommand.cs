using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Commands
{
    public class UpdateVendorMenuCommand : IRequest<VendorMenuDto>
    {
        public VendorMenuDto UpdateVendorMenu { get; set; }
    }

    public class UpdateVendorMenuCommandHandler : IRequestHandler<UpdateVendorMenuCommand, VendorMenuDto>
    {
        private readonly IVendorsMenuRepository vendorsMenuRepository;

        public UpdateVendorMenuCommandHandler(IVendorsMenuRepository vendorsMenuRepository)
        {
            this.vendorsMenuRepository = vendorsMenuRepository;
        }

        public async Task<VendorMenuDto> Handle(UpdateVendorMenuCommand request, CancellationToken cancellationToken)
        {
            return await vendorsMenuRepository.UpdateVendorMenus(request.UpdateVendorMenu);
        }
    }
}

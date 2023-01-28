using MediatR;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Commands
{
    public class DeleteVendorMenuCommand : IRequest<bool>
    {
        public string MenuId { get; set; }
    }

    public class DeleteVendorMenuCommandHandler : IRequestHandler<DeleteVendorMenuCommand, bool>
    {
        private readonly IVendorsMenuRepository vendorMenuRepository;

        public DeleteVendorMenuCommandHandler(IVendorsMenuRepository vendorMenuRepository)
        {
            this.vendorMenuRepository = vendorMenuRepository;
        }

        public async Task<bool> Handle(DeleteVendorMenuCommand request, CancellationToken cancellationToken)
        {
            return await vendorMenuRepository.DeleteVendorMenu(request.MenuId);
        }
    }
}

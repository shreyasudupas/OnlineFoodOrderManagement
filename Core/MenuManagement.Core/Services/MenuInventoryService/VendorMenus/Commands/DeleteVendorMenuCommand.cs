using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Commands
{
    public class DeleteVendorMenuCommand : IRequest<bool>
    {
        public string MenuId { get; set; }
    }

    public class DeleteVendorMenuCommandHandler : IRequestHandler<DeleteVendorMenuCommand, bool>
    {
        private readonly IVendorsMenuRepository vendorMenuRepository;
        private readonly IMapper _mapper;

        public DeleteVendorMenuCommandHandler(IVendorsMenuRepository vendorMenuRepository, IMapper mapper)
        {
            this.vendorMenuRepository = vendorMenuRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteVendorMenuCommand request, CancellationToken cancellationToken)
        {
            return await vendorMenuRepository.DeleteVendorMenu(request.MenuId);
        }
    }
}

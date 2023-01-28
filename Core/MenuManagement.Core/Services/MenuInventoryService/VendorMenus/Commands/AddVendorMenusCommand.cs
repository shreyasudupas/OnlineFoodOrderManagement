using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Command
{
    public class AddVendorMenusCommand : IRequest<VendorMenuDto>
    {
        public VendorMenuDto Menu { get; set; }
    }

    public class AddVendorMenusCommandHandler : IRequestHandler<AddVendorMenusCommand, VendorMenuDto>
    {
        private readonly IVendorsMenuRepository _menuRepository;

        public AddVendorMenusCommandHandler(IVendorsMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public Task<VendorMenuDto> Handle(AddVendorMenusCommand request, CancellationToken cancellationToken)
        {
            return _menuRepository.AddVendorMenus(request.Menu);
        }
    }
}

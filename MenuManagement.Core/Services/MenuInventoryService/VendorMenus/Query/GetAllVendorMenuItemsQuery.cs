using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Command
{
    public class GetAllVendorMenuItemsQuery : IRequest<List<MenuItemsDto>>
    {
        public string VendorId { get; set; }
    }

    public class GetAllVendorMenuItemsQueryHandler : IRequestHandler<GetAllVendorMenuItemsQuery, List<MenuItemsDto>>
    {
        private readonly IVendorsMenuRepository _menuRepository;

        public GetAllVendorMenuItemsQueryHandler(IVendorsMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuItemsDto>> Handle(GetAllVendorMenuItemsQuery request, CancellationToken cancellationToken)
        {
            return await _menuRepository.GetAllVendorMenuItems(request.VendorId);
        }
    }
}

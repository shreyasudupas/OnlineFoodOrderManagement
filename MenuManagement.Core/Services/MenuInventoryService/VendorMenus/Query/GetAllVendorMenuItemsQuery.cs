using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Command
{
    public class GetAllVendorMenuItemsQuery : IRequest<List<VendorMenuDto>>
    {
        public string VendorId { get; set; }
    }

    public class GetAllVendorMenuItemsQueryHandler : IRequestHandler<GetAllVendorMenuItemsQuery, List<VendorMenuDto>>
    {
        private readonly IVendorsMenuRepository _menuRepository;

        public GetAllVendorMenuItemsQueryHandler(IVendorsMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<VendorMenuDto>> Handle(GetAllVendorMenuItemsQuery request, CancellationToken cancellationToken)
        {
            return await _menuRepository.GetAllVendorMenuByVendorId(request.VendorId);
        }
    }
}

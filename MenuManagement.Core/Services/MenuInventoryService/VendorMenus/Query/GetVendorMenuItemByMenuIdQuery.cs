using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Query
{
    public class GetVendorMenuItemByMenuIdQuery : IRequest<VendorMenuDto>
    {
        public string MenuId { get; set; }
    }

    public class GetVendorMenuItemByMenuIdQueryHandler : IRequestHandler<GetVendorMenuItemByMenuIdQuery, VendorMenuDto>
    {
        private readonly IVendorsMenuRepository vendorsMenuRepository;

        public GetVendorMenuItemByMenuIdQueryHandler(IVendorsMenuRepository vendorsMenuRepository)
        {
            this.vendorsMenuRepository = vendorsMenuRepository;
        }

        public async Task<VendorMenuDto> Handle(GetVendorMenuItemByMenuIdQuery request, CancellationToken cancellationToken)
        {
            return await vendorsMenuRepository.GetVendorMenusByMenuId(request.MenuId);
        }
    }
}

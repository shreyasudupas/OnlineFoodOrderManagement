using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Command
{
    public class GetAllVendorMenusQuery : IRequest<List<VendorMenuDto>>
    {
    }

    public class GetAllVendorMenusQueryHandler : IRequestHandler<GetAllVendorMenusQuery, List<VendorMenuDto>>
    {
        private readonly IVendorsMenuRepository _menuRepository;

        public GetAllVendorMenusQueryHandler(IVendorsMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<VendorMenuDto>> Handle(GetAllVendorMenusQuery request, CancellationToken cancellationToken)
        {
            return await _menuRepository.GetAllMenu();
        }
    }
}

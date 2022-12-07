using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.MenuDetails
{
    public class MenuQuery : IRequest<List<MenuDto>>
    {
    }

    public class MenuQueryHandler : IRequestHandler<MenuQuery, List<MenuDto>>
    {
        private readonly IMenuRepository _menuRepository;

        public MenuQueryHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuDto>> Handle(MenuQuery request, CancellationToken cancellationToken)
        {
            return await _menuRepository.GetAllMenu();
        }
    }
}

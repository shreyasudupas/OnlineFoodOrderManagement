using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.GetMenuItemDetail
{
    public class GetMenuItemQuery : IRequest<List<MenuItemsDto>>
    {
        public string Id { get; set; }
    }

    public class GetMenuItemHandlerHandler : IRequestHandler<GetMenuItemQuery, List<MenuItemsDto>>
    {
        private readonly IMenuRepository _menuRepository;

        public GetMenuItemHandlerHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuItemsDto>> Handle(GetMenuItemQuery request, CancellationToken cancellationToken)
        {
            return await _menuRepository.GetAllMenuItem(request.Id);
        }
    }
}

using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.AddMenuItem
{
    public class AddMenuItemCommand : IRequest<MenuItemsDto>
    {
        public string MenuId { get; set; }
        public MenuItemsDto NewMenuItem { get; set; }
    }

    public class AddMenuItemCommandHandler : IRequestHandler<AddMenuItemCommand, MenuItemsDto>
    {
        private readonly IMenuRepository _menuRepository;

        public AddMenuItemCommandHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<MenuItemsDto> Handle(AddMenuItemCommand request, CancellationToken cancellationToken)
        {
            return await _menuRepository.AddMenuItem(request.MenuId,request.NewMenuItem);
        }
    }
}

using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.AddMenu
{
    public class AddMenuCommand : IRequest<MenuDto>
    {
        public MenuDto Menu { get; set; }
    }

    public class AddMenuCommandHandler : IRequestHandler<AddMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;

        public AddMenuCommandHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public Task<MenuDto> Handle(AddMenuCommand request, CancellationToken cancellationToken)
        {
            return _menuRepository.AddMenu(request.Menu);
        }
    }
}

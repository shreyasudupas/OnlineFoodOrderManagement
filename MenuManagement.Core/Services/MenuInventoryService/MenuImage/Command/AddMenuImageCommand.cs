using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.MenuImage.Command
{
    public class AddMenuImageCommand : IRequest<MenuImageDto>
    {
        public MenuImageDto MenuImageItem { get; set; }
    }

    public class AddMenuImageCommandHandler : IRequestHandler<AddMenuImageCommand, MenuImageDto>
    {
        private readonly IMenuImagesRepository _menuImagesRepository;

        public AddMenuImageCommandHandler(IMenuImagesRepository menuImagesRepository)
        {
            _menuImagesRepository = menuImagesRepository;
        }

        public async Task<MenuImageDto> Handle(AddMenuImageCommand request, CancellationToken cancellationToken)
        {
            return await _menuImagesRepository.AddMenuImage(request.MenuImageItem);
        }
    }
}

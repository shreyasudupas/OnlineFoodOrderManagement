using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.MenuImage.Command
{
    public class UploadImageCommand : IRequest<MenuImageDto>
    {
        public MenuImageDto MenuImageItem { get; set; }
    }

    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, MenuImageDto>
    {
        private readonly IMenuImagesRepository _menuImagesRepository;

        public UploadImageCommandHandler(IMenuImagesRepository menuImagesRepository)
        {
            _menuImagesRepository = menuImagesRepository;
        }

        public async Task<MenuImageDto> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            return await _menuImagesRepository.AddMenuImage(request.MenuImageItem);
        }
    }
}

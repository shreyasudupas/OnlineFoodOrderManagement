using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.MenuImage.Command
{
    public class UpdateMenuImagesCommand : IRequest<MenuImageDto>
    {
        public MenuImageDto UpdateMenuImage { get; set; }
    }

    public class UpdateMenuImagesCommandHandler : IRequestHandler<UpdateMenuImagesCommand, MenuImageDto>
    {
        private readonly IMenuImagesRepository menuImagesRepository;

        public UpdateMenuImagesCommandHandler(IMenuImagesRepository menuImagesRepository)
        {
            this.menuImagesRepository = menuImagesRepository;
        }

        public async Task<MenuImageDto> Handle(UpdateMenuImagesCommand request, CancellationToken cancellationToken)
        {
            return await menuImagesRepository.UpdateMenuImage(request.UpdateMenuImage);
        }
    }
}

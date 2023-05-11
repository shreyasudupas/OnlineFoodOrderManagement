using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.MenuImage.Command
{
    public class AddMenuImageCommand : IRequest<MenuImageDto>
    {
        public MenuImageDto MenuImageItem { get; set; }
    }

    public class AddMenuImageCommandHandler : IRequestHandler<AddMenuImageCommand, MenuImageDto>
    {
        private readonly IMenuImagesRepository _menuImagesRepository;
        private readonly IMapper _mapper;


        public AddMenuImageCommandHandler(IMenuImagesRepository menuImagesRepository,
            IMapper mapper)
        {
            _menuImagesRepository = menuImagesRepository;
            _mapper = mapper;
        }

        public async Task<MenuImageDto> Handle(AddMenuImageCommand request, CancellationToken cancellationToken)
        {
            var result = await _menuImagesRepository.AddMenuImage(request.MenuImageItem);
            if (result != null)
            {
                var mapToDoModel = _mapper.Map<MenuImageDto>(result);
                return mapToDoModel;
            }
            else
                return null;
        }
    }
}

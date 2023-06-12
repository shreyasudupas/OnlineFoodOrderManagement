using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.MenuImage.Command
{
    public class DeleteImageByImageIdCommand : IRequest<MenuImageDto>
    {
        public string ImageId { get; set; }
    }

    public class DeleteImageByImageIdCommandHandler : IRequestHandler<DeleteImageByImageIdCommand, MenuImageDto>
    {
        private readonly IMapper _mapper;
        private readonly IMenuImagesRepository _menuImagesRepository;

        public DeleteImageByImageIdCommandHandler(
            IMenuImagesRepository menuImagesRepository, IMapper mapper)
        {
            _menuImagesRepository = menuImagesRepository;
            _mapper = mapper;
        }

        public async Task<MenuImageDto> Handle(DeleteImageByImageIdCommand request, CancellationToken cancellationToken)
        {
            var data = await _menuImagesRepository.DeleteImageById(request.ImageId);

            var mapToDto = _mapper.Map<MenuImageDto>(data);
            return mapToDto;
        }
    }
}

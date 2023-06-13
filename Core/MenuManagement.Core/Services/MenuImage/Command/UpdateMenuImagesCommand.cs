using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuImage.Command
{
    public class UpdateMenuImagesCommand : IRequest<MenuImageDto>
    {
        public MenuImageDto UpdateMenuImage { get; set; }
    }

    public class UpdateMenuImagesCommandHandler : IRequestHandler<UpdateMenuImagesCommand, MenuImageDto>
    {
        private readonly IMenuImagesRepository menuImagesRepository;
        private readonly IMapper _mapper;

        public UpdateMenuImagesCommandHandler(IMenuImagesRepository menuImagesRepository,
            IMapper mapper)
        {
            this.menuImagesRepository = menuImagesRepository;
            _mapper = mapper;
        }

        public async Task<MenuImageDto> Handle(UpdateMenuImagesCommand request, CancellationToken cancellationToken)
        {
            var result = await menuImagesRepository.UpdateMenuImage(request.UpdateMenuImage);
            if (result != null)
            {
                var mapToDto = _mapper.Map<MenuImageDto>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}

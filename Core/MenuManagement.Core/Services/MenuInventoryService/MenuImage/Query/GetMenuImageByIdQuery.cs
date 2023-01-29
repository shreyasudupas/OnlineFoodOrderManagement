using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.MenuImage.Query
{
    public class GetMenuImageByIdQuery : IRequest<MenuImageDto>
    {
        public string Id { get; set; }
    }

    public class GetMenuImageByIdQueryHandler : IRequestHandler<GetMenuImageByIdQuery, MenuImageDto>
    {
        private readonly IMenuImagesRepository menuImageRespository;
        private readonly IMapper _mapper;

        public GetMenuImageByIdQueryHandler(IMenuImagesRepository menuImageRespository,
            IMapper mapper)
        {
            this.menuImageRespository = menuImageRespository;
            _mapper = mapper;
        }

        public async Task<MenuImageDto> Handle(GetMenuImageByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await menuImageRespository.GetMenuImagesById(request.Id);
            if(result != null)
            {
                var mapToDto = _mapper.Map<MenuImageDto>(result);
                return mapToDto;
            }
            else
            {
                return null;
            }
        }
    }
}

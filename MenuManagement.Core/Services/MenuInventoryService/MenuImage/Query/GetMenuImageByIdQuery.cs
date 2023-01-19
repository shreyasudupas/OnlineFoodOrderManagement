using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Core.Services.MenuInventoryService.MenuImage.Query.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.MenuImage.Query
{
    public class GetMenuImageByIdQuery : IRequest<MenuImageDto>
    {
        public string Id { get; set; }
    }

    public class GetMenuImageByIdQueryHandler : IRequestHandler<GetMenuImageByIdQuery, MenuImageDto>
    {
        private readonly IMenuImagesRepository menuImageRespository;

        public GetMenuImageByIdQueryHandler(IMenuImagesRepository menuImageRespository)
        {
            this.menuImageRespository = menuImageRespository;
        }

        public async Task<MenuImageDto> Handle(GetMenuImageByIdQuery request, CancellationToken cancellationToken)
        {
            return await menuImageRespository.GetMenuImagesById(request.Id);
        }
    }
}

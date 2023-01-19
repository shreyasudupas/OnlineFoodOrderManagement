using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Core.Mongo.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.MenuImage.Query
{
    public class GetAllMenuImagesQuery : IRequest<List<MenuImageDto>>
    {
        public Pagination Pagination { get; set; }
    }

    public class GetAllMenuImagesQueryHandler : IRequestHandler<GetAllMenuImagesQuery, List<MenuImageDto>>
    {
        private readonly IMenuImagesRepository menuImagesRespository;

        public GetAllMenuImagesQueryHandler(IMenuImagesRepository menuImagesRespository)
        {
            this.menuImagesRespository = menuImagesRespository;
        }

        public async Task<List<MenuImageDto>> Handle(GetAllMenuImagesQuery request, CancellationToken cancellationToken)
        {
            return await menuImagesRespository.GetAllMenuImages(request.Pagination);

        }
    }
}

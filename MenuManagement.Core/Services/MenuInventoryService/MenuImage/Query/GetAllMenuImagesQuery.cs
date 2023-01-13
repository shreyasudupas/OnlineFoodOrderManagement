using MediatR;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.MenuImage.Query
{
    public class ImageResponse
    {
        public string ItemName { get; set; }
        public string Data { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
    }

    public class GetAllMenuImagesQuery : IRequest<List<MenuImageDto>>
    {
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
            return await menuImagesRespository.GetAllMenuImages();
        }
    }
}

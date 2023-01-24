using MediatR;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Query
{
    public class GetMenuImagesByItemName : IRequest<List<MenuImageDto>>
    {
        public string SearchParam { get; set; }
    }

    public class GetMenuImagesByItemNameHandler : IRequestHandler<GetMenuImagesByItemName, List<MenuImageDto>>
    {
        private readonly IMenuImagesRepository menuImagesRepository;

        public GetMenuImagesByItemNameHandler(IMenuImagesRepository menuImagesRepository)
        {
            this.menuImagesRepository = menuImagesRepository;
        }

        public async Task<List<MenuImageDto>> Handle(GetMenuImagesByItemName request, CancellationToken cancellationToken)
        {
            var images = await menuImagesRepository.GetImagesBySearchParam(request.SearchParam);
            return images;
        }
    }
}

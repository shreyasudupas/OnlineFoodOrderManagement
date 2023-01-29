using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Query
{
    public class GetMenuImagesByItemName : IRequest<List<MenuImageDto>>
    {
        public string SearchParam { get; set; }
    }

    public class GetMenuImagesByItemNameHandler : IRequestHandler<GetMenuImagesByItemName, List<MenuImageDto>>
    {
        private readonly IMenuImagesRepository menuImagesRepository;
        private readonly IMapper _mapper;

        public GetMenuImagesByItemNameHandler(IMenuImagesRepository menuImagesRepository, IMapper mapper)
        {
            this.menuImagesRepository = menuImagesRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuImageDto>> Handle(GetMenuImagesByItemName request, CancellationToken cancellationToken)
        {
            var result = await menuImagesRepository.GetImagesBySearchParam(request.SearchParam);
            if(result != null)
            {
                var mapToDto = _mapper.Map<List<MenuImageDto>>(result);
                return mapToDto; 
            }
            else
                return null;
        }
    }
}

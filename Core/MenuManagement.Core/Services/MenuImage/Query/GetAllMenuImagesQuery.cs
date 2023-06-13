using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using MenuManagment.Mongo.Domain.Mongo.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuImage.Query
{
    public class GetAllMenuImagesQuery : IRequest<List<MenuImageDto>>
    {
        public Pagination Pagination { get; set; }
    }

    public class GetAllMenuImagesQueryHandler : IRequestHandler<GetAllMenuImagesQuery, List<MenuImageDto>>
    {
        private readonly IMenuImagesRepository menuImagesRespository;
        private readonly IMapper _mapper;

        public GetAllMenuImagesQueryHandler(IMenuImagesRepository menuImagesRespository,
            IMapper mapper)
        {
            this.menuImagesRespository = menuImagesRespository;
            _mapper = mapper;
        }

        public async Task<List<MenuImageDto>> Handle(GetAllMenuImagesQuery request, CancellationToken cancellationToken)
        {
            var result = await menuImagesRespository.GetAllMenuImages(request.Pagination);
            if (result == null)
            {
                return null;
            }
            else
            {
                var mapToModel = _mapper.Map<List<MenuImageDto>>(result);
                return mapToModel;
            }

        }
    }
}

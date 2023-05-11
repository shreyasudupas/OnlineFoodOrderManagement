using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.MenuImage.Query
{
    public class GetMenuImagesRecordCountQuery : IRequest<int>
    {
    }

    public class GetMenuImagesRecordCountQueryHandler : IRequestHandler<GetMenuImagesRecordCountQuery, int>
    {
        private readonly IMenuImagesRepository menuImagesRepository;
        private readonly IMapper _mapper;

        public GetMenuImagesRecordCountQueryHandler(IMenuImagesRepository menuImagesRepository, IMapper mapper)
        {
            this.menuImagesRepository = menuImagesRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(GetMenuImagesRecordCountQuery request, CancellationToken cancellationToken)
        {
            return await menuImagesRepository.GetMenuImageRecordCount();
        }
    }
}

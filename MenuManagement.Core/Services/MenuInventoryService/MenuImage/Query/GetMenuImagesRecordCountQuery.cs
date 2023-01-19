using MediatR;
using MenuManagement.Core.Mongo.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.MenuImage.Query
{
    public class GetMenuImagesRecordCountQuery : IRequest<int>
    {
    }

    public class GetMenuImagesRecordCountQueryHandler : IRequestHandler<GetMenuImagesRecordCountQuery, int>
    {
        private readonly IMenuImagesRepository menuImagesRepository;

        public GetMenuImagesRecordCountQueryHandler(IMenuImagesRepository menuImagesRepository)
        {
            this.menuImagesRepository = menuImagesRepository;
        }

        public async Task<int> Handle(GetMenuImagesRecordCountQuery request, CancellationToken cancellationToken)
        {
            return await menuImagesRepository.GetMenuImageRecordCount();
        }
    }
}

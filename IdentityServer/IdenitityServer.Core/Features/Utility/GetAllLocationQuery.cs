using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class GetAllLocationQuery : IRequest<List<RegisteredLocationReponse>>
    {
    }

    public class GetAllLocationQueryHandler : IRequestHandler<GetAllLocationQuery, List<RegisteredLocationReponse>>
    {
        private readonly IUtilsService _utilsService;

        public GetAllLocationQueryHandler(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }

        public Task<List<RegisteredLocationReponse>> Handle(GetAllLocationQuery request, CancellationToken cancellationToken)
        {
            return _utilsService.GetAllRegisteredLocation();
        }
    }
}

using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class GetAreaByCityIdQuery : IRequest<List<DropdownModel>>
    {
        public int CityId { get; set; }
    }

    public class GetAreaByCityIdQueryHandler : IRequestHandler<GetAreaByCityIdQuery, List<DropdownModel>>
    {
        private readonly IUserService _userService;

        public GetAreaByCityIdQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<DropdownModel>> Handle(GetAreaByCityIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _userService.GetLocationAreaById(request.CityId);

            return response;
        }
    }
}

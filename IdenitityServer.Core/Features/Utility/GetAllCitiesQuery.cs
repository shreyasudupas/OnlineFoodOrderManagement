using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class GetAllCitiesQuery : IRequest<List<DropdownModel>>
    {
    }

    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, List<DropdownModel>>
    {
        private readonly IUserService _userService;

        public GetAllCitiesQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<DropdownModel>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetAllCities();
        }
    }
}

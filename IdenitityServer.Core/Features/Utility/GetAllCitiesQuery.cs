using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class GetAllCitiesQuery : IRequest<List<AddressDropdownModel>>
    {
    }

    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, List<AddressDropdownModel>>
    {
        private readonly IUserService _userService;

        public GetAllCitiesQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<AddressDropdownModel>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetAllCities();
        }
    }
}

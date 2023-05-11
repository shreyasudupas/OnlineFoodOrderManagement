using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class GetCityByStateIdQuery : IRequest<List<DropdownModel>>
    {
        public int StateId { get; set; }
    }

    public class GetCityByStateIdHandler : IRequestHandler<GetCityByStateIdQuery, List<DropdownModel>>
    {
        private readonly IUserService _userService;

        public GetCityByStateIdHandler(IUserService userService)
        {
            _userService = userService;
        }
        public Task<List<DropdownModel>> Handle(GetCityByStateIdQuery request, CancellationToken cancellationToken)
        {
            var response = _userService.GetCityById(request.StateId);
            return response;
        }
    }
}

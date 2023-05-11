using IdenitityServer.Core.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class GetStateCityQuery : IRequest<(List<SelectListItem>,List<SelectListItem>)>
    {
    }

    public class GetStateCityQueryHandler : IRequestHandler<GetStateCityQuery, (List<SelectListItem>, List<SelectListItem>)>
    {
        private readonly IUtilsService _utilsService;

        public GetStateCityQueryHandler(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }
        public Task<(List<SelectListItem>, List<SelectListItem>)> Handle(GetStateCityQuery request, CancellationToken cancellationToken)
        {
            var cities = _utilsService.GetAllCities();
            var states = _utilsService.GetAllStates();
            return Task.FromResult((cities, states));
        }
    }
}

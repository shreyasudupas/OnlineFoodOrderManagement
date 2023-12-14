using IdenitityServer.Core.Common.Interfaces.HttpClient;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.LocationSearch.ForwardGeoCodingQuery
{
    public class ForwardGeoCodingQuery : IRequest<List<LocationResponse>>
    {
        public string LocationQuery { get; set; }
    }

    public class ForwardGeoCodingQueryHadler : IRequestHandler<ForwardGeoCodingQuery, List<LocationResponse>>
    {
        private readonly ILocationSearch _locationSearch;

        public ForwardGeoCodingQueryHadler(ILocationSearch locationSearch)
        {
            _locationSearch = locationSearch;
        }

        public async Task<List<LocationResponse>> Handle(ForwardGeoCodingQuery request, CancellationToken cancellationToken)
        {
            return await _locationSearch.GetForwardGeoCodingLocation(request.LocationQuery);
        }
    }
}

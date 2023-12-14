using IdenitityServer.Core.Common.Interfaces.HttpClient;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.LocationSearch.LocationReverseSearch
{
    public class LocationReverseSearchQuery : IRequest<LocationResponse>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class LocationReverseSearchQueryHandler : IRequestHandler<LocationReverseSearchQuery, LocationResponse>
    {
        private readonly ILocationSearch _locationSearch;

        public LocationReverseSearchQueryHandler(ILocationSearch locationSearch)
        {
            _locationSearch = locationSearch;
        }

        public async Task<LocationResponse> Handle(LocationReverseSearchQuery request, CancellationToken cancellationToken)
        {
            return await _locationSearch.GetRevereseLocation(request.Latitude,request.Longitude);
        }
    }
}

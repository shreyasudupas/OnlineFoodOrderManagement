using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.LocationSearch.ForwardGeoCodingQuery;
using IdenitityServer.Core.Features.LocationSearch.LocationReverseSearch;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.APIControllers
{
    [Authorize(LocalApi.PolicyName)]
    public class LocationController : BaseController
    {
        [HttpGet("/api/location/reverse")]
        public async Task<LocationResponse> GetReverseLocationDetails([FromQuery]double latitude,double longitude)
        {
            return await Mediator.Send(new LocationReverseSearchQuery
            {
                Latitude = latitude,
                Longitude = longitude
            });
        }

        [HttpGet("/api/location/forward")]
        public async Task<List<LocationResponse>> GetReverseLocationDetails([FromQuery] string locationQuery)
        {
            return await Mediator.Send(new ForwardGeoCodingQuery
            {
                LocationQuery = locationQuery
            });
        }
    }
}

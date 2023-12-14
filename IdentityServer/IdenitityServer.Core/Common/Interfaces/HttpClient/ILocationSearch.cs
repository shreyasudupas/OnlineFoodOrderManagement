using IdenitityServer.Core.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces.HttpClient;

public interface ILocationSearch
{
    Task<LocationResponse?> GetRevereseLocation(double latitude, double longitude);
    Task<List<LocationResponse>> GetForwardGeoCodingLocation(string locationName);
}

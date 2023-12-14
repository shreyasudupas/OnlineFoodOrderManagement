using IdenitityServer.Core.Common.Interfaces.HttpClient;
using IdenitityServer.Core.Domain.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.HttpClient
{
    public class LocationSearch : ILocationSearch
    {
        private readonly ILogger<LocationSearch> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public LocationSearch(ILogger<LocationSearch> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<LocationResponse?> GetRevereseLocation(double latitude,double longitude)
        {
            LocationResponse response = new();

            try
            {
                var httpClient = _httpClientFactory.CreateClient("ReverseGeoLocationClient");
                var queryParameters = new Dictionary<string, string>
                {
                    { "lat", Convert.ToString(latitude) },
                    { "lon", Convert.ToString(longitude) }
                };
                var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
                var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

                var responseMessage = await httpClient.GetAsync($"?" + queryString);

                if(responseMessage.IsSuccessStatusCode)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    if(!string.IsNullOrEmpty(result))
                    {
                        response = JsonConvert.DeserializeObject<LocationResponse>(result);
                        return response;
                    }
                    else
                    {
                        _logger.LogWarning("Result is empty");
                        return response;
                    }
                }
                else
                {
                    var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                    _logger.LogError("Location not found for the lat:{0} and long:{1}",latitude,longitude);
                    _logger.LogError("Location issue: {0}", errorResponse);
                    return response;
                }

            }
            catch(Exception ex)
            {
                _logger.LogError("Error has occured in Reverse GeoCode API : {0}",ex.Message);
                return null;
            }
        }

        public async Task<List<LocationResponse>> GetForwardGeoCodingLocation(string locationName)
        {
            List<LocationResponse> response = new();

            try
            {
                var httpClient = _httpClientFactory.CreateClient("ForwardGeoLocationClient");
                var queryParameters = new Dictionary<string, string>
                {
                    { "q", locationName }
                };
                var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
                var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

                var responseMessage = await httpClient.GetAsync($"?" + queryString);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(result))
                    {
                        response = JsonConvert.DeserializeObject<List<LocationResponse>>(result);
                        return response;
                    }
                    else
                    {
                        _logger.LogWarning("Result is empty");
                        return response;
                    }
                }
                else
                {
                    var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                    _logger.LogError("Location not found for the query {0}", locationName);
                    _logger.LogError("Location issue: {0}", errorResponse);
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error has occured in Reverse GeoCode API : {0}", ex.Message);
                return null;
            }
        }
    }
}

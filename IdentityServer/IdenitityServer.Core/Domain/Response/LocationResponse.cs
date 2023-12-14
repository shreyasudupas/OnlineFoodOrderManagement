using Newtonsoft.Json;

namespace IdenitityServer.Core.Domain.Response
{
    public class LocationResponse
    {
        [JsonProperty("place_id")]
        public  string PlaceId { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("address")]
        public LocationAddress Address { get; set; }
    }

    public class LocationAddress
    {
        [JsonProperty("road")]
        public string Road { get; set; }

        [JsonProperty("suburb")]
        public string Suburb { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postcode")]
        public string PostCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}

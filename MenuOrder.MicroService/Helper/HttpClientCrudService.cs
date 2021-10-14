using Common.Utility.Models;
using Identity.MicroService.Models.APIResponse;
using MenuOrder.MicroService.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MenuOrder.MicroService.Helper
{
    public class HttpClientCrudService
    {
        private readonly HttpClient _httpClient;

        public HttpClientCrudService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
        }

        public async Task<string> GetItemsAsync(string BaseUrl,string Action, string token ,UserHeaderInfo UserInfo)
        {
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("UserInfo",JsonConvert.SerializeObject(UserInfo));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(Action);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response>(content);

            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.Content.ToString();
            }
            else
            {
                //no item ,exception
                return null;
            }
        }
    }
}

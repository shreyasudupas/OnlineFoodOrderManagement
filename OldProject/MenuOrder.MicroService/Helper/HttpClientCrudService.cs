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
        private IHttpClientFactory _httpClientFactory;

        public HttpClientCrudService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetItemsAsync(string Action, string token ,UserHeaderInfo UserInfo)
        {
            var _httpClient = _httpClientFactory.CreateClient("Basket MicroService");
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

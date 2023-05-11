using MenuManagement.HttpClient.Domain.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.InventoryClient
{
    public class InventoryClientWrapper : IInventoryClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;


        public InventoryClientWrapper(IHttpClientFactory httpClientFactory,
            ILogger<InventoryClientWrapper> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string> PostApiCall<TData>(string url,string token,TData payload)
        {
            HttpResponseMessage responseMessage;
            var httpClient = _httpClientFactory.CreateClient("InventoryClient");

            try
            {
                var item = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                if (!string.IsNullOrEmpty(token))
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                responseMessage = await httpClient.PostAsync(url, item);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var data = await responseMessage.Content.ReadAsStringAsync();
                    return data;
                }
                else
                {
                    _logger.LogError($"Error in Posting the data to Inventory url:{url} with errors: {responseMessage.RequestMessage}");
                    return null;
                }
            }
            catch(Exception exception)
            {
                _logger.LogError($"Error occured in calling Post Inventory API, error: {exception.Message}");
                return null;
            }
        }
    }
}

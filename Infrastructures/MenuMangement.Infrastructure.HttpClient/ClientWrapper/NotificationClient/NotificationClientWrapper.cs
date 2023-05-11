using MenuManagement.HttpClient.Domain.Interface;
using MenuManagement.MessagingQueue.Core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.NotificationClient
{
    public class NotificationClientWrapper : INotificationClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        public NotificationClientWrapper(IHttpClientFactory httpClientFactory,
            ILogger<NotificationClientWrapper> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<NotificationModel> PostApiCall<TData>(string url,string token,TData payload)
        {
            HttpResponseMessage responseMessage;
            var httpClient = _httpClientFactory.CreateClient("NotificationClient");

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
                    var response = JsonConvert.DeserializeObject<NotificationModel>(data);

                    return response;
                }
                else
                {
                    _logger.LogError($"Error in Posting the data to Notification url:{url} with errors: {responseMessage.RequestMessage}");
                    return null;
                }
            }
            catch(Exception exception)
            {
                _logger.LogError($"Error occured in calling Post Notification API, error: {exception.Message}");
                return null;
            }
        }
    }
}

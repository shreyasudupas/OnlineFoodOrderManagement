using MenuManagement.HttpClient.Domain.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.IdentityServer
{
    public class IdsHttpClientWrapper : IIdsHttpClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public IdsHttpClientWrapper(IHttpClientFactory httpClientFactory,
            ILogger<IdsHttpClientWrapper> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> PostApiCallAsync<TData>(string url,TData payload,string token,string clientName)
        {
            HttpResponseMessage responseMessage;
            var httpClient = _httpClientFactory.CreateClient(clientName);

            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var item = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                if (!string.IsNullOrEmpty(token))
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                responseMessage = await httpClient.PostAsync(url, item);

                if(responseMessage.IsSuccessStatusCode)
                {
                    var data = await responseMessage.Content.ReadAsStringAsync();

                    return data;
                }
                else
                {
                    _logger.LogError($"Error in Posting the data to url:{url} with errors: {responseMessage.RequestMessage}");
                    return string.Empty;
                }
                

            }catch(Exception ex)
            {
                _logger.LogError($"Error calling the Post url {url} message:{ex.Message}");
                return string.Empty;
            }
        }

        public async Task<string> GetApiAccessToken()
        {
            HttpResponseMessage responseMessage;
            var httpClient = _httpClientFactory.CreateClient("IDSClient");

            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var body = new Dictionary<string, string>();
            body.Add("client_id", _configuration.GetSection("TokenConfiguration:ClientId").Value);
            body.Add("grant_type", _configuration.GetSection("TokenConfiguration:GrantType").Value);
            body.Add("scope", _configuration.GetSection("TokenConfiguration:Scopes").Value);

            try
            {
                responseMessage = await httpClient.PostAsync("",new FormUrlEncodedContent(body));

                if(responseMessage.IsSuccessStatusCode)
                {
                    return await responseMessage.Content.ReadAsStringAsync();
                }else
                {
                    _logger.LogError($"Status: {responseMessage.StatusCode}, Error message: {responseMessage.Content.ToString()}");
                    return string.Empty;
                }
            }
            catch(Exception ex )
            {
                _logger.LogError($"Error while Post API GetApiAccessToken, exception: {ex.Message}");
                return string.Empty;
            }
        }
    }
}

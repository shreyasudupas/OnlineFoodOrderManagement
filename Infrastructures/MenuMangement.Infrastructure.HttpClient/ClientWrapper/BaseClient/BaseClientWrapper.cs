﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient
{
    public class BaseClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        public BaseClientWrapper(IHttpClientFactory httpClientFactory,
            ILogger<BaseClientWrapper> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<TData?> PostApiCall<TData>(string url, string clientName, string token, TData payload)
        {
            HttpResponseMessage responseMessage;
            var httpClient = _httpClientFactory.CreateClient(clientName);

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
                    var response = JsonConvert.DeserializeObject<TData>(data);

                    return response;
                }
                else
                {
                    _logger.LogError($"Error in Posting the data to url:{url} with errors: {responseMessage.RequestMessage}");
                    return default;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occured in calling Post API, error: {exception.Message}");
                throw;
            }
        }

        public async Task<TData?> DeleteApiCall<TData>(string url, string clientName, string token)
        {
            HttpResponseMessage responseMessage;
            var httpClient = _httpClientFactory.CreateClient(clientName);

            try
            {
                if (!string.IsNullOrEmpty(token))
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                responseMessage = await httpClient.DeleteAsync(url);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var data = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<TData>(data);

                    return response;
                }
                else
                {
                    _logger.LogError($"Error in Delete the data to url:{url} with errors: {responseMessage.RequestMessage}");
                    return default;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occured in calling Deelte API, error: {exception.Message}");
                throw;
            }
        }
    }
}
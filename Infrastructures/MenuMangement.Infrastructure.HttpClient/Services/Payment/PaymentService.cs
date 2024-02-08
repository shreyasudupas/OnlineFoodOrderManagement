using MenuMangement.HttpClient.Domain.Interfaces.Services;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MenuMangement.Infrastructure.HttpClient.Services.Payment
{
    public class PaymentService : BaseClientWrapper, IPaymentService
    {
        private readonly ILogger _logger;
        public PaymentService(IHttpClientFactory httpClientFactory,
            ILogger<BaseClientWrapper> logger) : base(httpClientFactory, logger)
        {
            _logger = logger;
        }

        public async Task<bool> PaymentByRewardPoints(string url,string userId,string token, string bodyContent)
        {
            try
            {
                var clientName = "IDSClient";
                
                var resultContent = await PostApiCall(url, clientName, token, bodyContent);

                if (!string.IsNullOrEmpty(resultContent))
                {
                    var result = JsonConvert.DeserializeObject<bool>(resultContent);
                    return result;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}

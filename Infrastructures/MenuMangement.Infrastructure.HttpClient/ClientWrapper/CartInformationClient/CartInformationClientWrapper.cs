using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Core.Interfaces.Wrappers;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.CartInformationClient
{
    public class CartInformationClientWrapper : BaseClientWrapper, ICartInformationWrapper
    {
        private readonly ILogger<BaseClientWrapper> _logger;
        public CartInformationClientWrapper(IHttpClientFactory httpClientFactory
            ,ILogger<BaseClientWrapper> logger) 
            : base(httpClientFactory, logger)
        {
            _logger = logger;
        }

        public async Task<bool> ClearCartInformation(string userId,string token)
        {
            try
            {
                var clientName = "CartManagementClient";
                var result = await DeleteApiCall<bool>($"?userId={userId}", clientName, token);
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError("Error Occured in Clearing the Cart API: {0}",exception.Message);
                return false;
            }
        }
    }
}

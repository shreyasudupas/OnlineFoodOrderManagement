using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Saga.Orchestrator.Core.Interfaces.Services;

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

        public async Task<bool> PaymentByRewardPoints(string userId,string token, PaymentDetailDto payementDetail)
        {
            try
            {
                var clientName = "IDSClient";
                var body = new
                {
                    UserId = userId,
                    AmountToBeDebited = payementDetail.TotalPrice
                };
                var bodyContent = JsonConvert.SerializeObject(body);
                var resultContent = await PostApiCall("utility/update/points", clientName, token, bodyContent);

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

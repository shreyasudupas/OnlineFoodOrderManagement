using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Saga.Orchestrator.Core.Interfaces;

namespace MenuMangement.Infrastructure.HttpClient.Services
{
    public class PaymentService : BaseClientWrapper , IPaymentService
    {
        public PaymentService(IHttpClientFactory httpClientFactory,
            ILogger<BaseClientWrapper> logger) :base(httpClientFactory, logger)
        {
        }

        public async Task<bool> PaymentByRewardPoints(string userId,PaymentDetailDto payementDetail)
        {
            var clientName = "IDSClient";
            var body = new
            {
                UserId = userId,
                AmountToBeDebited = payementDetail.Price
            };
            var bodyContent = JsonConvert.SerializeObject(body);
            var resultContent = await PostApiCall<bool>("/utility/update/points",clientName,"", bodyContent);

            var result = JsonConvert.DeserializeObject<bool>(resultContent);
            return result;
        }
    }
}

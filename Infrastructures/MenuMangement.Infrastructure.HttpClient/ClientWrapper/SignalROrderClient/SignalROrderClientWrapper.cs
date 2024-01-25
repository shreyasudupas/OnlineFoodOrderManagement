using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.SignalROrderClient
{
    public class SignalROrderClientWrapper : BaseClientWrapper, ISignalROrderClientWrapper
    {
        private readonly ILogger _logger;
        public SignalROrderClientWrapper(IHttpClientFactory httpClientFactory,
            ILogger<BaseClientWrapper> logger) : base(httpClientFactory, logger)
        {
            _logger = logger;
        }

        public async Task<OrderInformationDto?> PostCallAsync(OrderInformationDto orderInformationDto,string token)
        {
            try
            {
                var clientName = "SignalRServiceClient";
                var payload = JsonSerializer.Serialize(orderInformationDto);
                var result = await PostApiCall($"recieveorder/new", clientName, token, payload);
                var orderInfoResult = JsonSerializer.Deserialize<OrderInformationDto>(result);
                return orderInformationDto;

            }
            catch (Exception exception)
            {
                _logger.LogError("Error Occured in Calling signalR Service Notification: {0}", exception.Message);
                return null;
            }
        }
    }
}

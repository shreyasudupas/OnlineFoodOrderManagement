using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Saga.Orchestrator.Core.Interfaces.Services;

namespace MenuMangement.Infrastructure.HttpClient.Services.Order;

public class OrderService : BaseClientWrapper, IOrderService
{
    private readonly ILogger<BaseClientWrapper> _logger;
    public OrderService(IHttpClientFactory httpClientFactory,
        ILogger<BaseClientWrapper> logger) : base(httpClientFactory, logger)
    {
        _logger = logger;
    }

    public async Task<OrderInformationDto?> AddOrderInformation(OrderInformationDto orderInformation, string token)
    {
        try
        {
            var clientName = "OrderManagementClient";

            var body = new
            {
                orderInfo = orderInformation
            };

            var bodyContent = JsonConvert.SerializeObject(body);
            var result = await PostApiCall("", clientName, token, bodyContent);

            if (!string.IsNullOrEmpty(result))
            {
                var jsonResult = JsonConvert.DeserializeObject<OrderInformationDto>(result);
                return jsonResult;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error has occured in AddOrderInformation Message: {0}", ex.Message);
            return null;
        }
    }
}


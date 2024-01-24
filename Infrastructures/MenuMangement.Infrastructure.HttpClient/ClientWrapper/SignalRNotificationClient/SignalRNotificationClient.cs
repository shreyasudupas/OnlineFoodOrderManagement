using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
using MenuMangement.HttpClient.Domain.Models;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.SignalRNotificationClient
{
    public class SignalRNotificationClient : BaseClientWrapper , ISignalRNotificationClient
    {
        private readonly ILogger _logger;
        public SignalRNotificationClient(IHttpClientFactory httpClientFactory,
            ILogger<BaseClientWrapper> logger) : base(httpClientFactory, logger)
        {
            _logger = logger;
        }

        public async Task GetAsyncCall(NotificationSignalRRequest notificationSignalRRequest,string token)
        {
            try
            {
                var clientName = "SignalRServiceClient";
                var result = await GetApiCall($"count?fromUserId={notificationSignalRRequest.FromUserId}&isSendAll={notificationSignalRRequest.isSendAll}&toUserId={notificationSignalRRequest.ToUserId}&count={notificationSignalRRequest.NotificationCount}", clientName, token);
                
            }
            catch (Exception exception)
            {
                _logger.LogError("Error Occured in Calling signalR Service Notification: {0}", exception.Message);
            }
        }
    }
}

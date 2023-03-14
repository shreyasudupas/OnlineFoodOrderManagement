using MenuManagement.HttpClient.Domain.Interface;
using MenuManagement.HttpClient.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MenuManagement.Webjob.Core.Interfaces;
using MenuManagement.Webjob.Core.Models;

namespace MenuManagement.Webjob.Core.Services
{
    public class ProcessVendorRegistrationService : IProcessVendorRegistrationService
    {
        private readonly ILogger _logger;
        private readonly IIdsHttpClientWrapper _idsHttpClientWrapper;
        private readonly INotificationClientWrapper _notificationClientWrapper;

        public ProcessVendorRegistrationService(
            ILogger<ProcessVendorRegistrationService> logger,
            IIdsHttpClientWrapper idsHttpClientWrapper
,
            INotificationClientWrapper notificationClientWrapper)
        {
            _logger = logger;
            _idsHttpClientWrapper = idsHttpClientWrapper;
            _notificationClientWrapper = notificationClientWrapper;
        }

        public async Task ProcessVendorRegistration(string vendorModelMessage)
        {
            var vendorModel = new VendorModel();

            if (!string.IsNullOrEmpty(vendorModelMessage))
            {
                vendorModel = JsonConvert.DeserializeObject<VendorModel>(vendorModelMessage);

                if(vendorModel != null)
                {
                    _logger.LogInformation($"Vendor Information {JsonConvert.SerializeObject(vendorModelMessage)}");

                    var tokenResult = await _idsHttpClientWrapper.GetApiAccessToken();

                    if (!string.IsNullOrEmpty(tokenResult))
                    {
                        var token = JsonConvert.DeserializeObject<AccessTokenModel>(tokenResult);

                        //call notification API
                        var notificationModel = new NotificationModel
                        {
                            UserId = vendorModel?.Id,
                            Description = "Welcome user",
                            Role = "vendor",
                            SendAll = false,
                            Title = "Welcome",
                            Read = false,
                            RecordedTimeStamp = DateTime.Now
                        };
                        var notificationResponse = await _notificationClientWrapper.PostApiCall("", token?.AccessToken, notificationModel);

                        if (notificationResponse != null)
                        {
                            _logger.LogInformation("Vendor Notification");
                        }
                        else
                        {
                            //send back to dead-letter queue
                        }
                    }
                    else
                    {
                        _logger.LogError("IDS Token is emtpty");
                    }
                }
                else
                {
                    _logger.LogError("Vendor Deserilize Emtpy");
                }
            }
        }
    }
}

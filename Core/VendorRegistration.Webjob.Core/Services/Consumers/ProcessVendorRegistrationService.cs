using MenuManagement.HttpClient.Domain.Interface;
using MenuManagement.HttpClient.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MenuManagement.MessagingQueue.Core.Interfaces;
using MenuManagement.MessagingQueue.Core.Models;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagement.MessagingQueue.Core.Interfaces.Consumers;

namespace MenuManagement.MessagingQueue.Core.Services.Consumers
{
    public class ProcessVendorRegistrationService : IProcessVendorRegistrationService
    {
        private readonly ILogger _logger;
        private readonly IIdsHttpClientWrapper _idsHttpClientWrapper;
        private readonly INotificationClientWrapper _notificationClientWrapper;
        private readonly IInventoryClientWrapper _inventoryClientWrapper;

        public ProcessVendorRegistrationService(
            ILogger<ProcessVendorRegistrationService> logger,
            IIdsHttpClientWrapper idsHttpClientWrapper
,
            INotificationClientWrapper notificationClientWrapper,
            IInventoryClientWrapper inventoryClientWrapper)
        {
            _logger = logger;
            _idsHttpClientWrapper = idsHttpClientWrapper;
            _notificationClientWrapper = notificationClientWrapper;
            _inventoryClientWrapper = inventoryClientWrapper;
        }

        public async Task ProcessVendorRegistration(string vendorModelMessage)
        {
            var vendorModel = new VendorModel();

            if (!string.IsNullOrEmpty(vendorModelMessage))
            {
                vendorModel = JsonConvert.DeserializeObject<VendorModel>(vendorModelMessage);

                if (vendorModel != null)
                {
                    _logger.LogInformation($"Vendor Information {JsonConvert.SerializeObject(vendorModelMessage)}");

                    var tokenResult = await _idsHttpClientWrapper.GetApiAccessToken();

                    if (!string.IsNullOrEmpty(tokenResult))
                    {
                        var token = JsonConvert.DeserializeObject<AccessTokenModel>(tokenResult);

                        //call Inventory Vendor API
                        var vendorDetail = new VendorDto
                        {
                            VendorName = vendorModel.VendorName,
                            VendorDescription = vendorModel.VendorDescription,
                            Rating = 0,
                            State = vendorModel.State,
                            City = vendorModel.City,
                            Area = vendorModel.Area,
                            Coordinates = null,
                            AddressLine1 = vendorModel.Address,
                            Active = false //vendor has to login and confirm the changes
                        };
                        var vendorAddBody = new
                        {
                            VendorDetail = vendorDetail
                        };

                        var vendorResult = await _inventoryClientWrapper.PostApiCall("api/vendor", token?.AccessToken, vendorAddBody);

                        if (!string.IsNullOrEmpty(vendorResult))
                        {
                            var deserializeVendor = JsonConvert.DeserializeObject<VendorDto>(vendorResult);

                            //call notification API
                            var notificationModel = new NotificationModel
                            {
                                UserId = vendorModel?.UserId,
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
                            //send back to dead letter
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

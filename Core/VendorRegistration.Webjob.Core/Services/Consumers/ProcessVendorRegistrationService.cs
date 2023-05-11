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
                try
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

                            //Step 1: Add the Vendor details
                            var vendorResult = await _inventoryClientWrapper.PostApiCall("api/vendor", token?.AccessToken, vendorAddBody);

                            if (!string.IsNullOrEmpty(vendorResult))
                            {
                                var deserializeVendor = JsonConvert.DeserializeObject<VendorDto>(vendorResult);

                                //Step 2: add vendor address to IDS Server to map user to Address (default address)
                                var vendorAddressModel = new
                                {
                                    userId = vendorModel.UserId,
                                    userAddress = new
                                    {
                                        fullAddress = vendorModel.Address,
                                        city = vendorModel?.City,
                                        state = vendorModel?.State,
                                        area = vendorModel?.Area,
                                        isActive = true,
                                        applcationUserId = vendorModel?.UserId,
                                        vendorId = deserializeVendor?.Id,
                                        editable = true
                                    }
                                };
                                var vendorAddressResult = await _idsHttpClientWrapper.PostApiCallAsync("api/vendor-address", vendorAddressModel, token?.AccessToken, "IDSClient");

                                if(!string.IsNullOrEmpty(vendorAddressResult))
                                {
                                    _logger.LogInformation($"Successfully processed Vendor User Address API");

                                    //Step 3: Add Vendor UserID Mapping Table
                                    var vendorUserIdModel = new
                                    {
                                        newVendorUserMapping = new
                                        {
                                            id = 0,
                                            userId = vendorModel?.UserId,
                                            username = "",
                                            emailId = "",
                                            vendorId = deserializeVendor?.Id,
                                            enabled = false
                                        }
                                    };
                                    var vendorUserIdMapping = await _idsHttpClientWrapper.PostApiCallAsync("api/vendor-user-mapping", vendorUserIdModel, token?.AccessToken, "IDSClient");
                                }
                                else
                                {
                                    _logger.LogError($"Error occured when processing Vendor User Address API");
                                }

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
                                    _logger.LogInformation("Vendor Notification sent");
                                }
                                else
                                {
                                    //send back to dead-letter queue
                                }
                            }
                            else
                            {
                                //send back to dead letter
                                var error = "Unable to add Vendor info API";
                                _logger.LogError(error);
                                throw new Exception(error);
                            }

                        }
                        else
                        {
                            var error = "IDS Token is emtpty";
                            _logger.LogError(error);
                            throw new Exception(error);
                        }
                    }
                    else
                    {
                        var error = "Vendor Deserilize Emtpy";
                        _logger.LogError(error);
                        throw new Exception(error);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error has occured {ex.Message}");
                    throw new Exception("Error in Process Vendor Service", ex);
                }
            }
        }
    }
}

using MenuManagement.HttpClient.Domain.Interface;
using MenuManagement.HttpClient.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;
using VendorRegistration.Webjob.Core.Interfaces;
using VendorRegistration.Webjob.Core.Models;

namespace VendorRegistration.Webjob.Core.Services
{
    public class ProcessVendorRegistrationService : IProcessVendorRegistrationService
    {
        private readonly ILogger _logger;
        private readonly IIdsHttpClientWrapper _idsHttpClientWrapper;

        public ProcessVendorRegistrationService(
            ILogger<ProcessVendorRegistrationService> logger,
            IIdsHttpClientWrapper idsHttpClientWrapper
            )
        {
            _logger = logger;
            _idsHttpClientWrapper = idsHttpClientWrapper;
        }

        public async Task ProcessVendorRegistration(string vendorModelMessage)
        {
            var vendorModel = new VendorModel();

            if (!string.IsNullOrEmpty(vendorModelMessage))
            {
                vendorModel = JsonConvert.DeserializeObject<VendorModel>(vendorModelMessage);

                _logger.LogInformation($"Vendor Information {JsonConvert.SerializeObject(vendorModelMessage)}");

                var tokenResult = await _idsHttpClientWrapper.GetApiAccessToken();

                var token = JsonConvert.DeserializeObject<AccessTokenModel>(tokenResult);
            }
        }
    }
}

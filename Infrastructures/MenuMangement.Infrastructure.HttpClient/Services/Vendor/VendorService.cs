using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Saga.Orchestrator.Core.Interfaces.Services;

namespace MenuMangement.Infrastructure.HttpClient.Services.Vendor;
public class VendorService : BaseClientWrapper ,IVendorSerivce
{
    private readonly ILogger _logger;
    public VendorService(IHttpClientFactory httpClientFactory,
        ILogger<BaseClientWrapper> logger) : base(httpClientFactory, logger)
    {
        _logger = logger;
    }

    public async Task<VendorDto?> GetVendorById(string vendorId, string token)
    {
        try
        {
            if(string.IsNullOrEmpty(vendorId))
            {
                _logger.LogError("Vendor Id is Empty");
                return null;
            }
            else
            {
                var clientName = "VendorManagementClient";
                var resultFromApi = await GetApiCall($"vendor/{ vendorId}",clientName, token);
                if (string.IsNullOrEmpty(resultFromApi))
                {
                    _logger.LogError("Error Occured when processing the Vendor Get by Id for {0}",vendorId);
                    return null;
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<VendorDto>(resultFromApi);
                    return response;
                }
                
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }
}

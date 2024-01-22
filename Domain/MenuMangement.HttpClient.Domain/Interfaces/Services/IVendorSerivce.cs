using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;

namespace MenuMangement.HttpClient.Domain.Interfaces.Services;
public interface IVendorSerivce
{
    Task<VendorDto?> GetVendorById(string vendorId, string token);
}

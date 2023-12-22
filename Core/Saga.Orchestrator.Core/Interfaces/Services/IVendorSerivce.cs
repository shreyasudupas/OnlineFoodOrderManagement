using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;

namespace Saga.Orchestrator.Core.Interfaces.Services;
public interface IVendorSerivce
{
    Task<VendorDto?> GetVendorById(string vendorId, string token);
}

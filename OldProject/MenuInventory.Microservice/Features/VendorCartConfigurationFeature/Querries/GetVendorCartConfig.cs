using MediatR;
using MenuInventory.Microservice.Models.VendorCartConfiguration;

namespace MenuInventory.Microservice.Features.VendorCartConfigurationFeature.Querries
{
    public record GetVendorCartConfig(string vendorId):IRequest<VendorCartConfigurationResponse>
    {
    }
}

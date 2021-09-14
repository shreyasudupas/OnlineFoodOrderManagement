using MediatR;
using MenuInventory.Microservice.Models.Menu;

namespace MenuInventory.Microservice.Features.MenuFeature.Querries
{
    public record VendorIdRequest(string VendorId):IRequest<MenuListReposnse>;
    
}

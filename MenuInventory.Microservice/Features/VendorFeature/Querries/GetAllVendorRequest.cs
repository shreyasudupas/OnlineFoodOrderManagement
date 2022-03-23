using MediatR;
using MenuInventory.Microservice.Models.Vendor;
using System.Collections.Generic;

namespace MenuInventory.Microservice.Features.VendorFeature.Querries
{
    public class GetAllVendorRequest:IRequest<List<VendorListResponse>>
    {
    }
}

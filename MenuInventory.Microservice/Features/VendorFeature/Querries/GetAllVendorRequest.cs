using MediatR;
using MenuInventory.Microservice.Data;
using System.Collections.Generic;

namespace MenuInventory.Microservice.Features.VendorFeature.Querries
{
    public class GetAllVendorRequest:IRequest<List<VendorList>>
    {
    }
}

using MediatR;
using MenuInventory.Microservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Features.MenuFeature.Querries
{
    public record VendorIdRequest(int VendorId):IRequest<MenuDisplayList>;
    
}

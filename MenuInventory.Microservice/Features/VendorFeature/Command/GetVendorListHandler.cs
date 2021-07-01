using MediatR;
using MenuInventory.Microservice.Data;
using MenuInventory.Microservice.Data.Context;
using MenuInventory.Microservice.Features.VendorFeature.Querries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Features.VendorFeature.Command
{
    public class GetVendorListHandler : IRequestHandler<GetAllVendorRequest, List<VendorList>>
    {
        private readonly MenuInventoryContext _dbContext;

        public GetVendorListHandler(MenuInventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VendorList>> Handle(GetAllVendorRequest request, CancellationToken cancellationToken)
        {
            List<VendorList> Vendorlist = new List<VendorList>();
            Vendorlist = await _dbContext.VendorLists.Where(x => x.Id > 0).ToListAsync();

            return Vendorlist;
        }
    }
}

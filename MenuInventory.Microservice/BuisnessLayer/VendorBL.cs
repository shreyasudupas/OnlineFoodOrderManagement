using MenuDatabase.Data.Database;
using MenuInventory.Microservice.BuisnessLayer.IBuisnessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.BuisnessLayer
{
    public class VendorBL : IVendorBL
    {
        private readonly MenuOrderManagementContext _dbContext;

        public VendorBL(MenuOrderManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TblVendorList>> GetVendorListAsync()
        {
            List<TblVendorList> Vendorlist = new List<TblVendorList>();
            Vendorlist = await _dbContext.TblVendorLists.Where(x => x.VendorId > 0).ToListAsync();

            return Vendorlist;
        }
    }
}

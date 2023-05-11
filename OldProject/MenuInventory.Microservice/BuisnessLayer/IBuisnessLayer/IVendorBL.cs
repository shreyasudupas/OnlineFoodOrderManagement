using MenuDatabase.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.BuisnessLayer.IBuisnessLayer
{
    public interface IVendorBL
    {
        Task<List<TblVendorList>> GetVendorListAsync();
    }
}

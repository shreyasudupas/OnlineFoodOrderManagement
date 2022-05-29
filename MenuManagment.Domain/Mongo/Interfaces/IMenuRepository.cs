using MenuManagment.Domain.Mongo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Domain.Mongo.Interfaces
{
    public interface IMenuRepository
    {
        Task<string> GetVendorDetails_DisplayName(string VendorId, string SearchColumnName);
        Task<List<VendorDetail>> ListAllVendorDetails(string Locality);
        Task<VendorMenuDetail> ListVendorMenuDetails(string VendorId,string Location);
    }
}

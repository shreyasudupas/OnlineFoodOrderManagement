using MenuManagment.Mongo.Domain.Mongo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository
{
    public interface IVendorsMenuRepository
    {
        Task<VendorsMenus> AddVendorMenus(VendorsMenus vendorMenus);
        Task<List<VendorsMenus>> GetAllMenu();

        Task<List<VendorsMenus>> GetAllVendorMenuByVendorId(string VendorId);
        Task<VendorsMenus> GetVendorMenusByMenuId(string menuId);
        Task<VendorsMenus> UpdateVendorMenus(VendorsMenus vendorMenus);
        Task<bool> DeleteVendorMenu(string menuId);

        Task<bool> AddVendorMenuList(List<VendorsMenus> vendorMenus);
    }
}

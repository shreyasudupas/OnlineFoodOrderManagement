using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository
{
    public interface IVendorsMenuRepository
    {
        Task<VendorsMenus> AddVendorMenus(VendorMenuDto menu);
        Task<List<VendorsMenus>> GetAllMenu();

        Task<List<VendorsMenus>> GetAllVendorMenuByVendorId(string VendorId);
        Task<VendorsMenus> GetVendorMenusByMenuId(string menuId);
        Task<VendorsMenus> UpdateVendorMenus(VendorMenuDto menu);
        Task<bool> DeleteVendorMenu(string menuId);
    }
}

using MenuManagement.Core.Mongo.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IVendorsMenuRepository
    {
        Task<VendorMenuDto> AddVendorMenus(VendorMenuDto menu);
        Task<List<VendorMenuDto>> GetAllMenu();

        Task<List<VendorMenuDto>> GetAllVendorMenuByVendorId(string VendorId);
        Task<VendorMenuDto> GetVendorMenusByMenuId(string menuId);
        Task<VendorMenuDto> UpdateVendorMenus(VendorMenuDto menu);
        Task<bool> DeleteVendorMenu(string menuId);
    }
}

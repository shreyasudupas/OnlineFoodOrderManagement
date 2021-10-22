using Common.Mongo.Database.Data.Context;
using System.Threading.Tasks;

namespace MenuOrder.MicroService.Helper
{
    public class MenuConfigurationService
    {
        private readonly MenuRepository _menuRepository;

        public MenuConfigurationService(MenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<string> GetColumnNameBasedOnVendorId(string VendorId, string SearchColumnName)
        {
            return await _menuRepository.GetVendorDetails_DisplayName(VendorId, SearchColumnName);
            
        }
    }
}

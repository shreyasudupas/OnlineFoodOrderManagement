using System.Collections.Generic;

namespace MenuManagement.Core.Common.Models.InventoryService
{
    public class VendorMenuDto
    {
        public string Id { get; set; }

        public string VendorId { get; set; }

        public List<MenuItemsDto> Items { get; set; }

        public bool Disable { get; set; }
    }
}

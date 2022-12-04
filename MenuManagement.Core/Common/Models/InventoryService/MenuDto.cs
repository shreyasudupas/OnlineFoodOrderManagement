using System.Collections.Generic;

namespace MenuManagement.Core.Common.Models.InventoryService
{
    public class MenuDto
    {
        public string Id { get; set; }

        public string VendorId { get; set; }

        public List<MenuItemsDto> Items { get; set; }

        public bool Disable { get; set; }
    }
}

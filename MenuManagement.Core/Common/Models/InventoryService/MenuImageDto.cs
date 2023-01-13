using Microsoft.AspNetCore.Http;

namespace MenuManagement.Core.Common.Models.InventoryService
{
    public class MenuImageDto
    {
        public string Id { get; set; }

        public string ItemName { get; set; }

        public string FileName { get; set; }
        
        public string Description { get; set; }
        
        public string ImagePath { get; set; }
        public bool Active { get; set; }
    }
}

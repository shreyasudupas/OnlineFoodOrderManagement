using Microsoft.AspNetCore.Http;

namespace Inventory.Microservice.Core.Common.Models.InventoryService.Request
{
    public class MenuFormImageRequest
    {
        public string Id { get; set; }

        public string ItemName { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public bool Active { get; set; }
    }
}

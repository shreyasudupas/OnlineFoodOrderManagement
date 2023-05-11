namespace Inventory.Microservice.Core.Common.Models.InventoryService.Request
{
    public class MenuImageUpload
    {
        public string Id { get; set; }

        public string ItemName { get; set; }
        public string Description { get; set; }
        
        public bool Active { get; set; }

        public ImageModel Image { get; set; }
    }

    public class ImageModel
    {
        public string Data { get; set; } //This is in base 64 format
        public string Type { get; set; } //jpg or png
    }
}

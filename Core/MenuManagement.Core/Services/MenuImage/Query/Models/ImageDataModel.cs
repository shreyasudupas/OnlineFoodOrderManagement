namespace Inventory.Microservice.Core.Services.MenuImage.Query.Models
{
    public class ImageDataModel
    {
        public string Id { get; set; }
        public string ItemName { get; set; }
        public string Data { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }

        public string FileName { get; set; }
    }
}

namespace MenuManagement.Core.Common.Models.InventoryService
{
    public class MenuItemsDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PictureLocation { get; set; }

        public string Type { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }
    }
}
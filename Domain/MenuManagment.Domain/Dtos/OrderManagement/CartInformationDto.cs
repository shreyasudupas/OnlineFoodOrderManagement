using MenuManagment.Mongo.Domain.Entities.SubModel;
using System.Collections.Generic;

namespace MenuManagment.Mongo.Domain.Dtos.OrderManagement
{
    public class CartInformationDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public List<MenuItemDto> MenuItems { get; set; }

        public string CartStatus { get; set; }
    }

    public class MenuItemDto
    {
        public string MenuId { get; set; }

        public string VendorId { get; set; }

        public string ItemName { get; set; }

        public ImageModel Image { get; set; }

        public string FoodType { get; set; }

        public string Category { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public int Quantity { get; set; }
    }
}

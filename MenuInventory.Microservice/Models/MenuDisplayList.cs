using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Models
{
    public class MenuDisplayList
    {
        public List<MenuList> MenuItemList { get; set; }
        public List<MenuItemDetail> MenuItemDetails { get; set; }
    }

    public class MenuList
    {
        public long Id { get; set; }
        public string MenuItem { get; set; }
        public int Price { get; set; }
        public long VendorId { get; set; }
        public string MenuType { get; set; }
        public string ImagePath { get; set; }
        public int? OfferPrice { get; set; }
        public DateTime? CreatedDate { get; set; }

    }

    public class MenuItemDetail
    {
        public long Id { get; set; }
        public string MenuTypeName { get; set; }
        public string ImagePath { get; set; }
    }
}

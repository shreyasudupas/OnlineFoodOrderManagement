using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Data
{
    public class Menu:IEntityBase
    {
        public long Id { get; set; }
        public string MenuItem { get; set; }
        public int Price { get; set; }
        public long VendorId { get; set; }
        public long MenuTypeId { get; set; }
        public string ImagePath { get; set; }
        public int? OfferPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public  MenuType MenuType { get; set; }
        public  VendorList Vendor { get; set; }
        
    }
}

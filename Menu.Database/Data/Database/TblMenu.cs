using System;
using System.Collections.Generic;

#nullable disable

namespace MenuDatabase.Data.Database
{
    public partial class TblMenu
    {
        public long MenuId { get; set; }
        public string MenuItem { get; set; }
        public int Price { get; set; }
        public int VendorId { get; set; }
        public int MenuTypeId { get; set; }
        public string ImagePath { get; set; }
        public int? OfferPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual TblMenuType MenuType { get; set; }
        public virtual TblVendorList Vendor { get; set; }
        public ICollection<tblUserOrder> UserOrders { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace MenuDatabase.Data.Database
{
    public partial class TblVendorList
    {
        public TblVendorList()
        {
            TblMenus = new HashSet<TblMenu>();
        }

        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorDescription { get; set; }
        public int? VendorRating { get; set; }
        public string VendorImgLink { get; set; }

        public virtual ICollection<TblMenu> TblMenus { get; set; }
        public ICollection<tblUserOrder> UserOrders { get; set; }
    }
}

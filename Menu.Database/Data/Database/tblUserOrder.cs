using System;

namespace MenuDatabase.Data.Database
{
    public class tblUserOrder
    {
        public long UserOrderId { get; set; }
        public long UserId { get; set; }
        public int  VendorId { get; set; }
        public long MenuId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public tblUser User { get; set; }
        public TblVendorList VendorList { get; set; }
        public TblMenu Menu { get; set; }
    }
}
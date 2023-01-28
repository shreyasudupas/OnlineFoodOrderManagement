using System.Collections.Generic;

namespace MenuManagment.Domain.Mongo.Entities
{
    public class VendorCartDetails
    {
        public VendorCartDetails()
        {
            ColumnDetails = new List<ColumnDetail>();
        }
        public string Id { get; set; }
        public VendorDetail VendorDetails { get; set; }

        public List<ColumnDetail> ColumnDetails { get; set; }
    }
}

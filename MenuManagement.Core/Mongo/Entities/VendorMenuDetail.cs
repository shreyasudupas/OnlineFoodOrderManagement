using System.Collections.Generic;

namespace MenuManagement.Core.Mongo.Entities
{
    public class VendorMenuDetail
    {
        public List<MenuColumnDetail> ColumnDetail { get; set; }
        public List<object> Data { get; set; }
    }
}

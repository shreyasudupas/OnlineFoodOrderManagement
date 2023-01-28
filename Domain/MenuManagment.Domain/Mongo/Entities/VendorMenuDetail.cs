using System.Collections.Generic;

namespace MenuManagment.Domain.Mongo.Entities
{
    public class VendorMenuDetail
    {
        public List<MenuColumnDetail> ColumnDetail { get; set; }
        public List<object> Data { get; set; }
    }
}

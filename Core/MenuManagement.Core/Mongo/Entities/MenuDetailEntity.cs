
using MenuManagement.Core.Mongo.Entities;

namespace MenuManagement.Core.Entities
{
    public class MenuDetailEntity
    {
        public string Id { get; set; }

        public string VendorName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public int Rating { get; set; }

        public VendorDetail VendorDetails { get; set; }
    }
}

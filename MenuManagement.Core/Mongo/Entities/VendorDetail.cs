namespace MenuManagement.Core.Mongo.Entities
{
    public class VendorDetail
    {
        public string VendorId { get; set; }
        public string VendorName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string VendorArea { get; set; }

        public int Rating { get; set; }
    }
}

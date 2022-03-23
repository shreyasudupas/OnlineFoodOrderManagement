using MenuInventory.Microservice.Models.Vendor;
using System.Collections.Generic;

namespace MenuInventory.Microservice.Models.VendorCartConfiguration
{
    public class VendorCartConfigurationResponse
    {
        public VendorListResponse VendorDetails { get; set; }
        public List<ColumnDetails> ColumnDetails { get; set; }
    }
}

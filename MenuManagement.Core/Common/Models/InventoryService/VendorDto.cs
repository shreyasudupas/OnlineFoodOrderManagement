using System;
using System.Collections.Generic;

namespace MenuManagement.Core.Common.Models.InventoryService
{
    public class VendorDto
    {
        public string Id { get; set; }

        public string VendorName { get; set; }

        public string VendorDescription { get; set; }

        public string Category { get; set; }

        public string Type { get; set; }

        public int Rating { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public CoordinatesDto Coordinates { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public DateTime OpenTime { get; set; }

        public DateTime CloseTime { get; set; }

        public bool Active { get; set; }

    }

    public class CoordinatesDto
    {
        public double Latitude { get; set; }

        public double Longitute { get; set; }
    }
}

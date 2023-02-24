using System.Collections.Generic;

namespace MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos
{
    public class VendorDto
    {
        public VendorDto()
        {
            Categories = new List<CategoryDto>();
            CuisineType = new List<string>();
        }
        public string Id { get; set; }

        public string VendorName { get; set; }

        public string VendorDescription { get; set; }

        public List<CategoryDto> Categories { get; set; }

        public List<string> CuisineType { get; set; }

        public int Rating { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public CoordinatesDto Coordinates { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

        public bool Active { get; set; }

    }

    public class CoordinatesDto
    {
        public double Latitude { get; set; }

        public double Longitute { get; set; }
    }
}

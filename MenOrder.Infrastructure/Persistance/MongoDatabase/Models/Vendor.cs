using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Models
{
    public class Vendor : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("vendorName")]
        public string VendorName { get; set; }

        [BsonElement("vendorDescription")]
        public string VendorDescription { get; set; }

        [BsonElement("categories")]
        public List<Categories> Categories { get; set; }

        [BsonElement("cuisineTypes")]
        public List<string> CuisineType { get; set; }

        [BsonElement("rating")]
        public int Rating { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("area")]
        public string Area { get; set; }

        [BsonElement("coordinates")]
        public Coordinates Coordinates { get; set; }

        [BsonElement("addressLine1")]
        public string AddressLine1 { get; set; }

        [BsonElement("addressLine2")]
        public string AddressLine2 { get; set; }

        [BsonElement("openTimming")]
        public DateTime OpenTime { get; set; }

        [BsonElement("closeTiming")]
        public DateTime CloseTime { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }

    }

    public class Coordinates
    {
        [BsonElement("Latitude")]
        public double Latitude { get; set; }

        [BsonElement("Longitute")]
        public double Longitute { get; set; }
    }
}

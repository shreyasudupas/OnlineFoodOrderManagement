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

        [BsonElement("Vendor name")]
        public string VendorName { get; set; }

        [BsonElement("Vendor Description")]
        public string VendorDescription { get; set; }

        [BsonElement("Categories")]
        public List<Categories> Categories { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Rating")]
        public int Rating { get; set; }

        [BsonElement("State")]
        public string State { get; set; }

        [BsonElement("City")]
        public string City { get; set; }

        [BsonElement("Area")]
        public string Area { get; set; }

        [BsonElement("Coordinates")]
        public Coordinates Coordinates { get; set; }

        [BsonElement("AddressLine1")]
        public string AddressLine1 { get; set; }

        [BsonElement("AddressLine2")]
        public string AddressLine2 { get; set; }

        [BsonElement("Open Timming")]
        public DateTime OpenTime { get; set; }

        [BsonElement("Close Timing")]
        public DateTime CloseTime { get; set; }

        [BsonElement("Active")]
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

using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Models
{
    public class Menus : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("vendor name")]
        public string VendorName { get; set; }

        [BsonElement("vendor area")]
        public string VendorArea { get; set; }


        [BsonElement("description")]
        public string Description { get; set; }


        [BsonElement("vendor location")]
        public string Location { get; set; }


        [BsonElement("rating")]
        public int Rating { get; set; }


        [BsonElement("vendor details")]
        public VendorDetail VendorDetails { get; set; }

    }
    public class VendorDetail
    {
        [BsonElement("column details")]
        public List<ColumnDetail> ColumnDetails { get; set; }


        [BsonElement("data")]
        public List<object> Data { get; set; }
    }

    public class ColumnDetail : CommonDetails
    {
        [BsonElement("column name")]
        public string ColumnName { get; set; }



        [BsonElement("column description")]
        public string ColumnDescription { get; set; }


        [BsonElement("display name")]
        public string DisplayName { get; set; }


        [BsonElement("display screen")]
        public string Display { get; set; }
    }

    public class CommonDetails
    {
        [BsonElement("property type")]
        public string PropertyType { get; set; }
    }
}

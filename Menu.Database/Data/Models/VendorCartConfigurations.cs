using Common.Mongo.Database.Data.BaseContext;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Common.Mongo.Database.Data.Models
{
    [BsonIgnoreExtraElements]
    public class VendorCartConfigurations : IEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("vendor details")]
        public CartVendorDetail VendorDetails { get; set; }

        [BsonElement("column details")]
        public List<ColumnDetail> ColumnDetails { get; set; }
    }

    public class CartVendorDetail
    {
        [BsonElement("vendor id")]
        public string VendorId { get; set; }
        [BsonElement("vendor name")]
        public string VendorName { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("vendor location")]
        public string Location { get; set; }

        [BsonElement("rating")]
        public int Rating { get; set; }
    }
}

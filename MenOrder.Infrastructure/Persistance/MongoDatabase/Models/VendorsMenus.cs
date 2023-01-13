using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Models
{
    public class VendorsMenus : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("vendorId")]
        public string VendorId { get; set; }

        [BsonElement("itemName")]
        public string ItemName { get; set; }

        [BsonElement("imageLocation")]
        public string ImageLocation { get; set; }

        [BsonElement("foodType")]
        public string FoodType { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("discount")]
        public int Discount { get; set; }

        [BsonElement("rating")]
        public int Rating { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }

    }
}

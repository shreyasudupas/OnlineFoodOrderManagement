﻿using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagment.Mongo.Domain.Mongo.Entities
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

        [BsonElement("imageId")]
        public string ImageId { get; set; }

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

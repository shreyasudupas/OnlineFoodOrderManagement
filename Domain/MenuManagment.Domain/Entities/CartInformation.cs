using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MenuManagment.Mongo.Domain.Entities.SubModel;

namespace MenuManagment.Mongo.Domain.Entities
{
    public class CartInformation : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("menuItems")]
        public List<MenuItem> MenuItems { get; set; }

        [BsonElement("cartStatus")]
        public string CartStatus { get; set; }
    }

    public class MenuItem
    {
        [BsonElement("id")]
        public string MenuId { get; set; }

        [BsonElement("vendorId")]
        public string VendorId { get; set; }

        [BsonElement("itemName")]
        public string ItemName { get; set; }

        [BsonElement("image")]
        public ImageModel Image { get; set; }

        [BsonElement("foodType")]
        public string FoodType { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("discount")]
        public int Discount { get; set; }

        [BsonElement("quatity")]
        public int Quantity { get; set; }
    }
}

using MenuManagment.Mongo.Domain.Entities.SubModel;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MenuManagment.Mongo.Domain.Entities;

public sealed record VendorCategoryMenu
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("itemName")]
    public string ItemName { get; set; }

    [BsonElement("image")]
    public ImageModel Image { get; set; }

    [BsonElement("foodType")]
    public string FoodType { get; set; }

    [BsonElement("price")]
    public double Price { get; set; }

    [BsonElement("discount")]
    public int Discount { get; set; }

    [BsonElement("rating")]
    public int Rating { get; set; }

    [BsonElement("active")]
    public bool Active { get; set; }
}

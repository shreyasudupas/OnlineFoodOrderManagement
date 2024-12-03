using MenuManagment.Mongo.Domain.Entities.SubModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MenuManagment.Mongo.Domain.Dtos.Inventory;

public record VendorCategoryMenu
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("vendorName")]
    public string VendorName { get; set; }

    [BsonElement("categories")]
    public CategoryVendor Categories { get; set; }
}

public record CategoryVendor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("openTime")]
    private TimeSpan CategoryOpenTime { get; set; }

    [BsonIgnore]
    public string OpenTime
    {
        get { return (new DateTime() + CategoryOpenTime).ToString(); }
    }

    [BsonElement("closeTime")]
    private TimeSpan CategoryCloseTime { get; set; }

    [BsonIgnore]
    public string CloseTime
    {
        get { return (new DateTime() + CategoryCloseTime).ToString(); }
    }

    [BsonElement("active")]
    public bool Active { get; set; }

    [BsonElement("categoryReleaseDate")]
    private DateTime CategoryReleaseDate { get; set; }

    [BsonIgnore]
    public string ReleaseDate
    {
        get { return CategoryReleaseDate.ToLocalTime().ToString(); }
    }

    [BsonElement("menuLists")]
    public List<CategoryVendorMenu> MenuLists { get; set; } = new();
}

public record CategoryVendorMenu
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("itemName")]
    public string ItemName { get; set; }

    [BsonElement("foodType")]
    public string FoodType { get; set; }

    [BsonElement("price")]
    public int Price { get; set; }

    [BsonElement("discount")]
    public int Discount { get; set; }

    [BsonElement("active")]
    public bool Active { get; set; }

    [BsonElement("rating")]
    public int Rating { get; set; }

    [BsonElement("image")]
    public ImageModel Image { get; set; }
}

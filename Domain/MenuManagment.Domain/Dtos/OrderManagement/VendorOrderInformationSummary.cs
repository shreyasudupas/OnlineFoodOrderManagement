using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagment.Mongo.Domain.Dtos.OrderManagement;

public record VendorOrderInformationSummary
{
    [BsonElement("_id")]
    public string VendorId { get; init; }

    [BsonElement("count")]
    public int OrderCount { get; init; }

    [BsonElement("vendorName")]
    public string VendorName { get; init; }
}

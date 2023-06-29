using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagment.Mongo.Domain.Entities.SubModel
{
    public class ImageModel
    {
        [BsonElement("imageId")]
        public string ImageId { get; set; } = string.Empty;

        [BsonElement("imageFileName")]
        public string ImageFileName { get; set; } = string.Empty;
    }
}

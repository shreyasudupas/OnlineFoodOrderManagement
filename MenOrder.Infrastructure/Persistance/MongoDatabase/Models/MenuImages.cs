using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Models
{
    public class MenuImages : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("itemName")]
        public string ItemName { get; set; }

        [BsonElement("fileName")]
        public string FileName { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("imagePath")]
        public string ImagePath { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }
    }
}

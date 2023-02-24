using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagment.Mongo.Domain.Mongo.Entities
{
    public class VendorFoodType : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("typeName")]
        public string TypeName { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }
    }
}

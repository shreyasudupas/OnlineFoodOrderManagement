using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagment.Mongo.Domain.Mongo.Entities
{
    public class VendorCuisineType : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("cuisineName")]
        public string CuisineName { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }
    }
}

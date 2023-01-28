using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Models.Database
{
    public class VendorCuisineType: IEntity
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

using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Models
{
    public class VendorCuisineType: IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Cuisine Name")]
        public string CuisineName { get; set; }

        [BsonElement("Active")]
        public bool Active { get; set; }
    }
}

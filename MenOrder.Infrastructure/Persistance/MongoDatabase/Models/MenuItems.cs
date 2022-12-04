using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Models
{
    public class MenuItems : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Picture Location")]
        public string PictureLocation { get; set; }

        [BsonElement("Menu Type")]
        public string Type { get; set; }

        [BsonElement("Price")]
        public double Price { get; set; }

        [BsonElement("Discount")]
        public int Discount { get; set; }
    }
}

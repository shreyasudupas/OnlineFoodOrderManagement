using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Models
{
    public class Menus : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("VendorId")]
        public string VendorId { get; set; }

        [BsonElement("Items")]
        public List<MenuItems> Items { get; set; }

        [BsonElement("Disable")]
        public bool Disable { get; set; }

    }
}

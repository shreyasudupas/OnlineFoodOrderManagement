using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MenuManagment.Mongo.Domain.Mongo.Entities
{
    public class Categories : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("openTime")]
        public TimeSpan OpenTime { get; set; }

        [BsonElement("closeTime")]
        public TimeSpan CloseTime { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }
    }
}

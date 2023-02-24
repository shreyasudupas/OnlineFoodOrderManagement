using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MenuManagment.Mongo.Domain.Mongo.Entities
{
    public class Notifications : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }

        [BsonElement("notification_recorded_timestamp")]
        public DateTime RecordedTimeStamp { get; set; }

        [BsonElement("link")]
        public string Link { get; set; }

        [BsonElement("sendAll")]
        public bool SendAll { get; set; }

        [BsonElement("read")]
        public bool Read { get; set; }
    }
}

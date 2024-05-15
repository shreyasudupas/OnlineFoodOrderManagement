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

        [BsonElement("priority")]
        public string Priority { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("fromUserId")]
        public string FromUserId { get; set; }

        [BsonElement("toUserId")]
        public string ToUserId { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }

        [BsonElement("data")]
        public NotificationData Data { get; set; }

        [BsonElement("sendAll")]
        public bool SendAll { get; set; }

        [BsonElement("read")]
        public bool Read { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }
    }

    public class NotificationData
    {
        [BsonElement("uri")]
        public string Uri { get; set; }

        [BsonElement("request_type")]
        public string RequestType { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }
    }
}

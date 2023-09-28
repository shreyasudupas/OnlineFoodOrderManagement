using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using System;

namespace MenuManagment.Mongo.Domain.Entities
{
    public class OrderInformation : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("cartId")]
        public string CartId { get; set; }

        [BsonElement("menuItems")]
        public List<MenuItem> MenuItems { get; set; }

        [BsonElement("paymentDetails")]
        public PaymentOrderDetail PayementDetails { get; set; }

        [BsonElement("userDetails")]
        public UserOrderDetails UserDetails { get; set; }

        [BsonElement("orderPlaced")]
        public DateTime OrderPlaced { get; set; }

        [BsonElement("orderStatus")]
        public string OrderStatus { get; set; }
    }

    public class PaymentOrderDetail
    {
        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("selectedPayment")]
        public string SelectedPayment { get; set; }

        [BsonElement("methodOfDelivery")]
        public string MethodOfDelivery { get; set; }

        [BsonElement("paymentSuccess")]
        public bool PaymentSuccess { get; set; }
    }

    public class UserOrderDetails
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("fullAddress")]
        public string FullAddress { get; set; }

        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }
    }
}

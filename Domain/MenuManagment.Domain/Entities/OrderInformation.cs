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

        [BsonElement("totalPrice")]
        public double TotalPrice { get; set; }

        [BsonElement("paymentDetail")]
        public PaymentOrderDetail PaymentDetail { get; set; }

        [BsonElement("userDetail")]
        public UserOrderDetail UserDetail { get; set; }

        [BsonElement("orderPlacedDateTime")]
        public DateTime OrderPlacedDateTime { get; set; }

        [BsonElement("orderStatus")]
        public string OrderStatus { get; set; }

        [BsonElement("vendorDetail")]
        public VendorDetail VendorDetail { get; set; }
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

    public class UserOrderDetail
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("fullAddress")]
        public string FullAddress { get; set; }

        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }

        [BsonElement("area")]
        public string Area { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("emailId")]
        public string EmailId { get; set; }
    }

    public class VendorDetail
    {
        [BsonElement("vendorId")]
        public string VendorId { get; set; }

        [BsonElement("vendorName")]
        public string VendorName { get; set; }
    }
}

using MenuOrder.MicroService.Data.Context;
using MenuOrder.MicroService.Data.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MenuOrder.MicroService.Data
{
    public class Orders:IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public UserInfo UserInfo { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    public class UserInfo
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public UserAddress Address { get; set; }
        public string PictureLocation { get; set; }

    }

    public class UserAddress
    {
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class OrderItem
    {
        public string Id { get; set; }
        public string MenuItem { get; set; }
        public int Price { get; set; }
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public string MenuType { get; set; }
        public string ImagePath { get; set; }
        public string OfferPrice { get; set; }
        public string CreatedDate { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }
    }
}

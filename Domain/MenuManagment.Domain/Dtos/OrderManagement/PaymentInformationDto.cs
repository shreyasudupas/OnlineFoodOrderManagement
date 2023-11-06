using System.Collections.Generic;

namespace MenuManagment.Mongo.Domain.Dtos.OrderManagement
{
    public record PaymentInformationRecord
    {
        public string UserId { get; set; }
        public string CartId { get; set; }
        public List<MenuItemDto> MenuItems { get; set; } = new List<MenuItemDto>();
        public PaymentDetailDto PayementDetails { get; set; }
        public UserOrderDetails UserDetails { get; set; }
        public string OrderPlaced { get; set; }
        public string OrderStatus { get; set; }
    }

    public class PaymentDetailDto
    {
        public double Price { get; set; }
        public string SelectedPayment { get; set; }
        public string MethodOfDelivery { get; set; }
        public bool PaymentSuccess { get; set; }
    }

    public class UserOrderDetails
    {
        public string UserId { get; set; }
        public string FullAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

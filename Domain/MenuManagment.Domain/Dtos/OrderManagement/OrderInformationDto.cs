using MenuManagment.Mongo.Domain.Enum;
using System.Collections.Generic;

namespace MenuManagment.Mongo.Domain.Dtos.OrderManagement
{
    public class OrderInformationDto
    {
        public string Id { get; set; }

        public string CartId { get; set; }

        public List<MenuItemDto> MenuItems { get; set; }

        public PaymentOrderDetailDto PaymentDetails { get; set; }

        public UserOrderDetailsDto UserDetails { get; set; }

        public string OrderPlacedDateTime { get; set; }

        public OrderStatusEnum OrderStatus { get; set; }
    }

    public class PaymentOrderDetailDto
    {
        public double TotalPrice { get; set; }

        public string SelectedPayment { get; set; }

        public string MethodOfDelivery { get; set; }

        public bool PaymentSuccess { get; set; }
    }

    public class UserOrderDetailsDto
    {
        public string UserId { get; set; }

        public string FullAddress { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}

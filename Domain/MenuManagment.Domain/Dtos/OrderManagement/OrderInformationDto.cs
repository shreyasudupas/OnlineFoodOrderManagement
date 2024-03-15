using System;
using System.Collections.Generic;

namespace MenuManagment.Mongo.Domain.Dtos.OrderManagement
{
    public class OrderInformationDto
    {
        public string Id { get; set; }

        public string CartId { get; set; }

        public List<MenuItemDto> MenuItems { get; set; }
        public double TotalPrice { get; set; }

        public PaymentOrderDetailDto PaymentDetail { get; set; }

        public UserOrderDetailsDto UserDetail { get; set; }

        public OrderStatusDetailDto Status { get; set; }
        public VendorDetailDto VendorDetail { get; set; }
        public long UIOrderNumber { get; set; }
        public string CurrentOrderStatus { get; set; }
        public string OrderCancelledReason { get; set; } = null;
        public DateTime CreatedDate { get; set; }
    }

    public class PaymentOrderDetailDto
    {
        public double Price { get; set; }

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

        public string Area { get; set; }

        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
    }

    public class VendorDetailDto
    {
        public string VendorId { get; set; }

        public string VendorName { get; set; }
    }

    public class OrderStatusDetailDto
    {
        public string OrderPlaced { get; set; } = null;

        public string OrderInProgress { get; set; } = null;

        public string OrderReady { get; set; } = null;

        public string OrderDone { get; set; } = null;

        public string OrderCancelled { get; set; } = null;
    }
}

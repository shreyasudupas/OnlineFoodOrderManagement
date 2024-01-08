using System.Collections.Generic;

namespace MenuManagment.Mongo.Domain.Dtos.OrderManagement
{
    public sealed record PaymentInformationRecord
    {
        public string UserId { get; init; }
        public CartInformationRecord CartInfo { get; init; }
        public PaymentDetailDto PaymentInfo { get; init; }
        public UserAddressDetail UserAddress { get; init; }
    }

    public record CartInformationRecord
    {
        public string CartId { get; init; }
        public List<MenuItemDto> MenuItems { get; set; } = new List<MenuItemDto>();
    }

    public record PaymentDetailDto
    {
        public double TotalPrice { get; init; }
        public string SelectedPayment { get; init; }
        public string MethodOfDelivery { get; init; }
        public bool PaymentSuccess { get; init; }
    }

    public record UserAddressDetail
    {
        public string FullAddress { get; init; }
        public string City { get; init; }
        public string Area { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
    }
}

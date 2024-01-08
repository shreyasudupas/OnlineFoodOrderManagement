using MenuManagment.Mongo.Domain.Enum;

namespace OrderManagement.Microservice.Core.Querries.Orders.GetVendorOrders
{
    public record GetVendorByStatusRecord
    {
        public string VendorId { get; init; }
        public OrderStatusEnum OrderStatus { get; init; }
    }
}

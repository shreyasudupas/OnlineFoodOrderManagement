namespace OrderManagement.Microservice.Core.Querries.Orders.GetOrderCount
{
    public record GetOrderCountResponse(int OrderPlaced,int OrderInProgress,int OrderCancelled);
}

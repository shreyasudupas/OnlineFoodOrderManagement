using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Manager
{
    public interface IOrderManager
    {
        Task SendLatestOrderToClients(OrderInformationDto orderInformationDto);
        Task SendOrderCancellationUpdatesBackToVendor(OrderInformationDto orderInformationDto);
    }
}

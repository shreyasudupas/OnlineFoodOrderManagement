using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Services
{
    public interface IOrderManager
    {
        Task SendLatestOrderToClients(OrderInformationDto orderInformationDto);
    }
}

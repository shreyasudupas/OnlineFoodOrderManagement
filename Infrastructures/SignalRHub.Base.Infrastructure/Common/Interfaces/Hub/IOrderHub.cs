using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Hub
{
    public interface IOrderHub
    {
        Task PublishLatestOrderInformation(OrderInformationDto orderInformation);
    }
}

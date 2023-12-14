using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace Saga.Orchestrator.Core.Interfaces.Services;

public interface IOrderService
{
    Task<OrderInformationDto?> AddOrderInformation(OrderInformationDto orderInformation, string token);
}

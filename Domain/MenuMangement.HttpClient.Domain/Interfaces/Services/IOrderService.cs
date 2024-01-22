using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace MenuMangement.HttpClient.Domain.Interfaces.Services;

public interface IOrderService
{
    Task<OrderInformationDto?> AddOrderInformation(OrderInformationDto orderInformation, string token);
}
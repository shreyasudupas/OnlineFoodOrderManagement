using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace MenuMangement.HttpClient.Domain.Interfaces.Wrappers
{
    public interface ISignalROrderClientWrapper
    {
        Task<OrderInformationDto?> PostCallAsync(OrderInformationDto orderInformationDto, string token);
    }
}

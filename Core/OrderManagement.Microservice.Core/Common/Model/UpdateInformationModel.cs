using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace OrderManagement.Microservice.Core.Common.Model
{
    public record UpdateInformationModel
    {
        public OrderInformationDto OrderInfo { get; init; } = new();
    }
}

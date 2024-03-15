using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Interfaces.Repository.Order
{
    public interface IOrderRepository
    {
        Task<OrderInformation?> AddOrderInformation(OrderInformation order);
        Task<OrderInformation?> UpdateOrderInformation(OrderInformation order);
        Task<List<OrderInformation>> GetOrderListInformationBasedOnStatus(string[] status);

        Task<List<OrderInformation>> GetAllOrdersBasedOnUserId(string userId);
        Task<List<OrderInformation>> GetOrderInformationBasedOnOrderStatus(string vendorId, string[] orderStatus);
        Task<long> GetNextUIBasedOrderNumber(string vendorId);

    }
}

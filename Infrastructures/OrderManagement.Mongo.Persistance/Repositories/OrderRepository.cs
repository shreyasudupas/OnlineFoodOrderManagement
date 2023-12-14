using MenuManagment.Mongo.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Repositories;
using MongoDB.Driver;
using MongoDb.Shared.Persistance.Extensions;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;
using MenuManagment.Mongo.Domain.Enum;

namespace OrderManagement.Mongo.Persistance.Repositories
{
    public class OrderRepository : BaseRepository<OrderInformation> , IOrderRepository
    {
        private ILogger<OrderRepository> _logger;
        public OrderRepository(IMongoDBContext context,
            ILogger<OrderRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<List<OrderInformation>> GetOrderListInformationBasedOnStatus(string status)
        {
            var builder = Builders<OrderInformation>.Filter;
            var filter = builder.Eq(order=>order.OrderStatus, status);
            var orders = await GetListByFilterDefinition(filter);

            return orders;
        }

        public async Task<OrderInformation?> AddOrderInformation(OrderInformation order)
        {
            if(string.IsNullOrEmpty(order.Id))
            {
                await CreateOneDocument(order);

                var getOrder = await GetByFilter(order=>order.OrderStatus == OrderStatusEnum.OrderPlaced.ToString() 
                    && order.UserDetails.UserId == order.UserDetails.UserId);
                return getOrder;
            }
            else
            {
                _logger.LogError("Order Id must be empty");
                return null;
            }
        }

        public async Task<OrderInformation?> UpdateOrderInformation(OrderInformation order)
        {
            var orderDetail = await GetById(order.Id);
            if(orderDetail != null)
            {
                var filter = Builders<OrderInformation>.Filter.Eq(x => x.Id, order.Id);
                var update = Builders<OrderInformation>.Update.ApplyMultiFields(order);

                var result = await UpdateOneDocument(filter, update);

                if (result.IsAcknowledged)
                {
                    return order;
                }
                else
                    return null;
            }
            else
            {
                _logger.LogError($"Order Id: {order.Id} not present");
                return null;
            }
        }
    }
}

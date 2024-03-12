using MenuManagment.Mongo.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Repositories;
using MongoDB.Driver;
using MongoDb.Shared.Persistance.Extensions;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;
using MenuManagment.Mongo.Domain.Enum;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Options;

namespace OrderManagement.Mongo.Persistance.Repositories
{
    public class OrderRepository : BaseRepository<OrderInformation> , IOrderRepository
    {
        private ILogger<OrderRepository> _logger;
        public OrderRepository(IOptions<MongoDatabaseConfiguration> mongoDatabaseSettings,
            //IMongoDBContext context
            ILogger<OrderRepository> logger) : base(mongoDatabaseSettings)
        {
            _logger = logger;
        }

        public async Task<List<OrderInformation>> GetOrderListInformationBasedOnStatus(string status)
        {
            var builder = Builders<OrderInformation>.Filter;
            var filter = BuildOrderStatusEqualFilter(status,builder);
            var orders = await GetListByFilterDefinition(filter);

            return orders;
        }

        public async Task<OrderInformation?> AddOrderInformation(OrderInformation order)
        {
            if(string.IsNullOrEmpty(order.Id))
            {
                await CreateOneDocument(order);

                var getOrder = await GetDocumentByFilter(order=> order.UIOrderNumber == order.UIOrderNumber
                    && order.UserDetail.UserId == order.UserDetail.UserId);
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

        public async Task<List<OrderInformation>> GetAllOrdersBasedOnUserId(string userId)
        {
            List<OrderInformation> result = new();
            if(!string.IsNullOrEmpty(userId))
            {
                var builder = Builders<OrderInformation>.Filter;
                var filter = builder.Eq(order => order.UserDetail.UserId, userId);

                result = await ListDocumentsByDesendingSortFilter(filter, x => x.OrderStatusDetails.OrderPlaced);
            }
            
            return result;
        }

        public async Task<List<OrderInformation>?> GetOrderInformationBasedOnOrderStatus(string vendorId,string orderStatus)
        {
            List<OrderInformation> result = new();
            if(!string.IsNullOrEmpty(vendorId) && !string.IsNullOrEmpty(orderStatus))
            {
                var builder = Builders<OrderInformation>.Filter;
                var filter = builder.Eq(order => order.VendorDetail.VendorId, vendorId)
                    & BuildOrderStatusDateLesserThanEqualFilter(orderStatus, DateTime.Now, builder);

                result = await ListDocumentsByDesendingSortFilter(filter,o=>o.CreatedDate);
                return result;
            }
            else
            {
                _logger.LogError("vendorId: {0} is empty or orderStatus: {1} is empty",vendorId,orderStatus);
                return null;
            }
        }

        //This function is to get the order number which will be shown in the UI for User to identify the order Numbers
        public async Task<long> GetNextUIBasedOrderNumber(string vendorId)
        {
            long uiOrderNumber = 0;
            var builder = Builders<OrderInformation>.Filter;
            var filter = builder.Eq(order => order.VendorDetail.VendorId, vendorId);

            var getAllOrder = await ListDocumentsByDesendingSortFilter(filter, order => order.OrderStatusDetails.OrderPlaced);
            var getLatestOrder = getAllOrder.FirstOrDefault();
            if (getLatestOrder != null)
            {
                uiOrderNumber = getLatestOrder.UIOrderNumber + 1;
            }
            else
            {
                uiOrderNumber++;
            }

            return uiOrderNumber;
        }

        private FilterDefinition<OrderInformation>? BuildOrderStatusEqualFilter(string orderStatus
            ,FilterDefinitionBuilder<OrderInformation> filterBuilder)
        {
            var orderStatusEvaluation = Enum.GetName(typeof(OrderStatusEnum), orderStatus);

            if(orderStatusEvaluation != null)
            {
                switch(orderStatusEvaluation)
                {
                    case nameof(OrderStatusEnum.OrderPlaced):
                        return filterBuilder.Eq(order => order.OrderStatusDetails.OrderInProgress, null);

                    case nameof(OrderStatusEnum.OrderInProgress):
                        return filterBuilder.Eq(order => order.OrderStatusDetails.OrderReady, null) & 
                            filterBuilder.Ne(order => order.OrderStatusDetails.OrderPlaced, null);

                    case nameof(OrderStatusEnum.OrderReady):
                        return filterBuilder.Eq(order => order.OrderStatusDetails.OrderDone, null) &
                            filterBuilder.Ne(order => order.OrderStatusDetails.OrderInProgress, null);

                    case nameof(OrderStatusEnum.OrderDone):
                        return filterBuilder.Eq(order => order.OrderStatusDetails.OrderCancelled, null) &
                            filterBuilder.Ne(order => order.OrderStatusDetails.OrderReady, null);

                    case nameof(OrderStatusEnum.OrderCancelled):
                        return filterBuilder.Ne(order => order.OrderStatusDetails.OrderCancelled, null);

                    default: return null;
                }
            }
            else
            {
                throw new Exception("OrderStatus not existing");
            }
        }

        private FilterDefinition<OrderInformation>? BuildOrderStatusDateLesserThanEqualFilter(string orderStatus,DateTime orderPlacedDate, FilterDefinitionBuilder<OrderInformation> filterBuilder)
        {
            var orderStatusEvaluation = Enum.Parse(typeof(OrderStatusEnum), orderStatus);

            if (orderStatusEvaluation != null)
            {
                var orderStatusEnumName = Enum.GetName(typeof(OrderStatusEnum), orderStatusEvaluation);
                switch (orderStatusEnumName)
                {
                    case nameof(OrderStatusEnum.OrderPlaced):
                        return filterBuilder.Lte(order => order.OrderStatusDetails.OrderPlaced, orderPlacedDate);

                    case nameof(OrderStatusEnum.OrderInProgress):
                        return filterBuilder.Lte(order => order.OrderStatusDetails.OrderInProgress, orderPlacedDate);

                    case nameof(OrderStatusEnum.OrderReady):
                        return filterBuilder.Lte(order => order.OrderStatusDetails.OrderReady, orderPlacedDate);

                    case nameof(OrderStatusEnum.OrderDone):
                        return filterBuilder.Lte(order => order.OrderStatusDetails.OrderDone, orderPlacedDate);

                    case nameof(OrderStatusEnum.OrderCancelled):
                        return filterBuilder.Lte(order => order.OrderStatusDetails.OrderCancelled, orderPlacedDate);

                    default: throw new Exception("OrderStatus does not match the filter");
                }
            }
            else
            {
                throw new Exception("OrderStatus not existing");
            }
        }
    }
}

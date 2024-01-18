﻿using MenuManagment.Mongo.Domain.Entities;
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

                var getOrder = await GetDocumentByFilter(order=>order.OrderStatus == OrderStatusEnum.OrderPlaced.ToString() 
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

                result = await ListDocumentsByDesendingSortFilter(filter, x => x.OrderPlacedDateTime);
            }
            
            return result;
        }

        public async Task<List<OrderInformation>?> GetOrderInformationBasedOnOrderStatus(string vendorId,string orderStatus)
        {
            List<OrderInformation> result = new();
            if(!string.IsNullOrEmpty(vendorId) && !string.IsNullOrEmpty(orderStatus))
            {
                var builder = Builders<OrderInformation>.Filter;
                var filter = builder.Eq(order => order.VendorDetail.VendorId, vendorId) & builder.Eq(order => order.OrderStatus, orderStatus)
                    & builder.Lte(order=>order.OrderPlacedDateTime, DateTime.Now);

                result = await ListDocumentsByDesendingSortFilter(filter,o=>o.OrderPlacedDateTime);
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

            var getAllOrder = await ListDocumentsByDesendingSortFilter(filter, order => order.OrderPlacedDateTime);
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
    }
}

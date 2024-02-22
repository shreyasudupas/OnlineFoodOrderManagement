using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Enum;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.AddOrderInformation;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderInformation;
using OrderManagement.Microservice.Core.Querries.Orders.GetAllOrders;
using OrderManagement.Microservice.Core.Querries.Orders.GetVendorOrders;

namespace MenuMangement.OrderManagement.API.Controller
{
    public class OrderController : BaseController
    {
        [HttpGet("/api/order/list")]
        public async Task<List<OrderInformationDto>> GetAllOrders([FromQuery] string userId)
        {
            return await Mediator.Send(new GetOrdersQuery { UserId = userId });
        }

        [HttpPost("/api/order")]
        public async Task<OrderInformationDto> AddOrderInformation([FromBody] AddOrderInformationCommand addOrderInformationCommand)
        {
            return await Mediator.Send(addOrderInformationCommand);
        }

        [HttpPut("/api/order")]
        public async Task<OrderInformationDto> UpdateOrderInformation([FromBody] UpdateOrderInformationCommand updateOrderInformationCommand)
        {
            return await Mediator.Send(updateOrderInformationCommand);
        }

        [HttpPost("/api/order/list/status")]
        public async Task<List<OrderInformationDto>> GetOrdersBasedOnStatus([FromBody] GetOrderStatusByVendorDto getOrderStatusByVendor)
        {
            List<OrderStatusEnum> orderStatusList = new();
            foreach (var status in getOrderStatusByVendor.VendorStatus)
            {
                orderStatusList.Add((OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), status));
            }

            var query = new GetVendorOrdersBasedOnOrderStatusQuery { 
                VendorStatus =  new GetVendorByStatusRecord
                {
                    VendorId = getOrderStatusByVendor.VendorId,
                    OrderStatus = orderStatusList.ToArray()
                }
            };

            var result = await Mediator.Send(query);
            return result;
        }
    }
}

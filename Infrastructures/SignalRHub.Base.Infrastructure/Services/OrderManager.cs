using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using Microsoft.AspNetCore.SignalR;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using SignalRHub.Base.Infrastructure.Hubs;

namespace SignalRHub.Base.Infrastructure.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IVendorUserManager _vendorUserManager;
        private readonly IHubContext<OrdersHub,IOrderHub> _orderHub;

        public OrderManager(IVendorUserManager vendorUserManager,
            IHubContext<OrdersHub, IOrderHub> orderHub)
        {
            _vendorUserManager = vendorUserManager;
            _orderHub = orderHub;
        }

        public async Task SendLatestOrderToClients(OrderInformationDto orderInformationDto)
        {
            var vendorUsers = _vendorUserManager.GetAllVendorUsersConnections().Where(v => v.VendorId == orderInformationDto.VendorDetail.VendorId)
                .ToList();

            if(vendorUsers.Count > 0)
            {
                var userConnectionIds = vendorUsers.Select(v=>v.ConnectionId).ToList();

                await _orderHub.Clients.Clients(userConnectionIds).PublishLatestOrderInformation(orderInformationDto);
            }
        }
    }
}

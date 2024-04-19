using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using Microsoft.AspNetCore.SignalR;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Manager;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Models;
using SignalRHub.Base.Infrastructure.Hubs;

namespace SignalRHub.Base.Infrastructure.Manager
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
            var vendorUsers = GeConnectedtVendorUsers(orderInformationDto.VendorDetail.VendorId);

            if (vendorUsers.Count > 0)
            {
                var userConnectionIds = vendorUsers.Select(v => v.ConnectionId).ToList();

                await _orderHub.Clients.Clients(userConnectionIds).PublishLatestOrderInformation(orderInformationDto);
            }
        }

        private List<VendorHubUser> GeConnectedtVendorUsers(string vendorId)
        {
            return _vendorUserManager.GetAllVendorUsersConnections()
                            .Where(v => v.VendorId == vendorId)
                            .ToList();
        }

        public async Task SendOrderCancellationUpdatesBackToVendor(OrderInformationDto orderInformationDto)
        {
            var vendorUsers = GeConnectedtVendorUsers(orderInformationDto.VendorDetail.VendorId);
            if(vendorUsers.Count > 0)
            {
                var userConnectionIds = vendorUsers.Select(v => v.ConnectionId).ToList();

                await _orderHub.Clients.Clients(userConnectionIds).PublishCancelOrder(orderInformationDto);
            }
        }
    }
}

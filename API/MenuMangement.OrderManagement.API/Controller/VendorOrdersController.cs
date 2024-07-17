using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using OrderManagement.Microservice.Core.Querries.Orders.GetAllVendorOrderSummary;

namespace MenuMangement.OrderManagement.API.Controller
{
    public class VendorOrdersController : BaseController
    {
        [HttpGet("/api/vendorOrder/summary")]
        public async Task<List<VendorOrderInformationSummary>> GetVendorOrdersSummaryList()
        {
            return await Mediator.Send(new GetAllVendorOrderSummaryQuery());
        }
    }
}

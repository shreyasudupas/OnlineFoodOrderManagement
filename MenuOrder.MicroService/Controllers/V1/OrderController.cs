using MediatR;
using MenuOrder.MicroService.Data;
using MenuOrder.MicroService.Features.MenuOrderFeature.Querries;
using MenuOrder.MicroService.Models;
using MicroService.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuOrder.MicroService.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository orderRepository;
        private readonly IMediator _mediator;
        public OrderController(OrderRepository orderRepository, IMediator mediator)
        {
            this.orderRepository = orderRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await orderRepository.GetAllItems();
            return Ok(result);
        }
        /// <summary>
        /// Sets the user orders by getting the info from the basekt service
        /// </summary>
        /// <returns>user Id if ok</returns>

        [HttpPost]
        public async Task<IActionResult> UserOrder()
        {
            APIResponse response = new APIResponse();
            var GetUserInfoFromheader = HttpContext.Request.Headers["UserInfo"];
            if (GetUserInfoFromheader.Where(x => x.Length > 0).Any())
            {
                var UserInfo = JsonConvert.DeserializeObject<UserHeaderInfo>(GetUserInfoFromheader);
                var result = await _mediator.Send(new AddUserMenuOrder() { UserName = UserInfo.Username });
                if (result.UserId != null)
                {
                    response.Response = 200;
                    response.Content = result;
                }
                else
                {
                    response.Response = 404;
                }
            }
            else
            {
                response.Response = 404;
                response.Exception = "Header Empty";
            }
            
            return Ok(response);
        }
    }
}

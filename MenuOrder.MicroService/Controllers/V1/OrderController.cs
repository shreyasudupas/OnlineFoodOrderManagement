using Identity.MicroService.Models.APIResponse;
using MediatR;
using MenuOrder.MicroService.BackgroundServiceTasks;
using MenuOrder.MicroService.Data;
using MenuOrder.MicroService.Features.MenuOrderFeature.Querries;
using MenuOrder.MicroService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace MenuOrder.MicroService.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository orderRepository;
        private readonly IMediator _mediator;
        public IBackgroundTaskQueue _queue;
        private IServiceScopeFactory _serviceScopeFactory;
        public OrderController(OrderRepository orderRepository, IMediator mediator, IBackgroundTaskQueue queue, IServiceScopeFactory serviceScopeFactory)
        {
            this.orderRepository = orderRepository;
            _mediator = mediator;
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
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
        public async Task<Response> UserOrder()
        {
            var GetUserInfoFromheader = HttpContext.Request.Headers["UserInfo"];
            if (GetUserInfoFromheader.Where(x => x.Length > 0).Any())
            {
                _queue.QueueBackgroundWorkItem(async token =>
                {
                    var UserInfo = JsonConvert.DeserializeObject<UserHeaderInfo>(GetUserInfoFromheader);
                    using(var scope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(new AddUserMenuOrder() { UserName = UserInfo.Username });
                        if (result.UserId != null)
                        {
                            //response.Response = 200;
                        }
                        else
                        {
                            //response.Response = 404;
                        }
                    }
                    
                });

                return new Response(HttpStatusCode.Accepted, "Order Sent Successfully", null);
            }
            else
            {
                return new Response(HttpStatusCode.BadRequest, null, "Header Empty");
            }
        }

        /// <summary>
        /// Get Payment Information
        /// </summary>
        /// <returns>Return User Information</returns>
        [HttpGet]
        public async Task<Response> GetUserPaymentDetails()
        {
            var GetUserInfoFromheader = HttpContext.Request.Headers["UserInfo"];

            var accessToken =  HttpContext.Request.Headers["Authorization"].ToString();

            if (GetUserInfoFromheader.Where(x => x.Length > 0).Any())
            {
                var UserInfo = JsonConvert.DeserializeObject<UserHeaderInfo>(GetUserInfoFromheader);

                var result = await _mediator.Send(new GetUserPaymentDetails { UserInfo = UserInfo , Token = accessToken.Replace("Bearer","")  });

                return new Response(HttpStatusCode.OK, result, null);
            }
            else
            {
                return new Response(HttpStatusCode.BadRequest, null, "Header User details is emtpy");
            }
                
        }
    }
}

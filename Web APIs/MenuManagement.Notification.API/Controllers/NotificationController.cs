using MenuManagment.Microservice.Core.Dtos;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using Notification.Microservice.Core.Querries.GetAllNotificationByUserId;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Notification.API.Controllers
{
    public class NotificationController : BaseController
    {
        [HttpGet("/api/notification/{userId}")]
        public async Task<List<NotificationDto>> GetAllNotification(string userId,[FromQuery] int skip,int limit)
        {
            return await Mediator.Send(new GetAllNotificationByUserIdQuery { UserId = userId , Skip = skip , Limit = limit });
        }
    }
}

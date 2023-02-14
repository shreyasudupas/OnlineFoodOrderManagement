using MenuManagment.Microservice.Core.Dtos;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Microservice.Core.Command.AddNotification;
using Notification.Microservice.Core.Command.UpdateNotification;
using Notification.Microservice.Core.Querries.GetAllNotificationByUserId;
using Notification.Microservice.Core.Querries.GetNotificationCount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Notification.API.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        [HttpGet("/api/notification/{userId}")]
        public async Task<List<NotificationDto>> GetAllNotification(string userId,[FromQuery] int skip,int limit)
        {
            return await Mediator.Send(new GetAllNotificationByUserIdQuery { UserId = userId , Skip = skip , Limit = limit });
        }

        [AllowAnonymous]
        [HttpPost("/api/notification")]
        public async Task<NotificationDto> AddNotification(NotificationDto notification)
        {
            return await Mediator.Send(new AddNotificationCommand { NewNotification = notification });
        }

        [HttpPut("/api/notification")]
        public async Task<NotificationDto> UpdateNotification(NotificationDto notification)
        {
            return await Mediator.Send(new UpdateNotificationCommand { Notification = notification });
        }

        [AllowAnonymous]
        [HttpGet("/api/notification/{userId}/count")]
        public async Task<int> NewNotificationCount(string userId)
        {
            return await Mediator.Send(new GetNotificationCountQuery { Userid = userId });
        }
    }
}

using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Hubs;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Notification.Microservice.Core.Command.AddNotification;
using Notification.Microservice.Core.Command.UpdateNotification;
using Notification.Microservice.Core.Querries.GetAllNotificationByUserId;
using Notification.Microservice.Core.Querries.GetNotificationCount;
using Notification.Microservice.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Notification.API.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly TimerControl _timerControl;

        public NotificationController(IHubContext<NotificationHub> notificationHub, TimerControl timerControl)
        {
            _notificationHub = notificationHub;
            _timerControl = timerControl;
        }

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
        public async Task<IActionResult> NewNotificationCount(string userId)
        {
            if (!_timerControl.IsTimerStarted)
                _timerControl.ScheduleTimer(async () => 
                    await _notificationHub.Clients.All
                    .SendAsync("SendUserNotificationCount", Mediator.Send(new GetNotificationCountQuery { Userid = userId })),2000);

                return Ok(new { Message = "Synchronized" });
        }
    }
}

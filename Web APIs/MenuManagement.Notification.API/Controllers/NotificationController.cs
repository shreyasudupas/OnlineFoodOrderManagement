using MediatR;
using MenuManagment.Microservice.Core.Dtos;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Notification.Microservice.Core.Command.AddNotification;
using Notification.Microservice.Core.Command.UpdateNotification;
using Notification.Microservice.Core.Domain.Service;
using Notification.Microservice.Core.Hub;
using Notification.Microservice.Core.Interface;
using Notification.Microservice.Core.Querries.GetAllNotificationByUserId;
using Notification.Microservice.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Notification.API.Controllers
{
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;
        private readonly TimerControl _timerControl;
        private readonly IProfileUser _profileUser;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;

        public NotificationController(
            IHubContext<NotificationHub, INotificationHub> notificationHub
            , TimerControl timerControl
            , IProfileUser profileUser
            , IMediator mediator
            , INotificationService notificationService
            )
        {
            _notificationHub = notificationHub;
            _timerControl = timerControl;
            _profileUser = profileUser;
            _mediator = mediator;
            _notificationService = notificationService;
        }

        [HttpGet("/api/notification/{userId}")]
        public async Task<List<NotificationDto>> GetAllNotification(string userId,[FromQuery] int skip,int limit)
        {
            return await _mediator.Send(new GetAllNotificationByUserIdQuery { UserId = userId , Skip = skip , Limit = limit });
        }

        [AllowAnonymous]
        [HttpPost("/api/notification")]
        public async Task<NotificationDto> AddNotification([FromBody] NotificationDto notification)
        {
            return await _mediator.Send(new AddNotificationCommand { NewNotification = notification });
        }

        [HttpPut("/api/notification")]
        public async Task<NotificationDto> UpdateNotification([FromBody]NotificationDto notification)
        {
            return await _mediator.Send(new UpdateNotificationCommand { Notification = notification });
        }

        [AllowAnonymous]
        [HttpGet("/api/notification/{userId}/count")]
        public async Task<IActionResult> NewNotificationCount(string userId)
        {
            var result = _notificationService.GetNotificationCount(userId);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("/api/notification/test")]
        public async Task<IActionResult> TestAPi([FromQuery]int count,string connectionId)
        {
            await _notificationHub.Clients.Client(connectionId).SendUserNotification(count);
            return Ok();
        }
    }
}

using MediatR;
using MenuManagment.Microservice.Core.Dtos;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Microservice.Core.Command.AddNotification;
using Notification.Microservice.Core.Command.UpdateNotification;
using Notification.Microservice.Core.Interface;
using Notification.Microservice.Core.Querries.GetAllNotification;
using Notification.Microservice.Core.Querries.GetAllNotificationByUserId;
using Notification.Microservice.Core.Querries.GetNotificationById;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Notification.API.Controllers
{
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(
            IProfileUser profileUser
            , IMediator mediator
            , INotificationService notificationService
            )
        {
            _mediator = mediator;
        }

        [HttpGet("/api/notification/list")]
        public async Task<List<NotificationDto>> GetAllNotification()
        {
            return await _mediator.Send(new GetAllNotificationQuery());
        }

        [HttpGet("/api/notification/{userId}")]
        public async Task<List<NotificationDto>> GetAllUserBasedNotification(string userId,[FromQuery] int skip,int limit)
        {
            return await _mediator.Send(new GetAllNotificationByUserIdQuery { UserId = userId , Skip = skip , Limit = limit });
        }

        //[AllowAnonymous]
        [HttpPost("/api/notification")]
        public async Task<NotificationDto> AddNotification([FromBody]NotificationDto notification)
        {
            return await _mediator.Send(new AddNotificationCommand { NewNotification = notification });
        }

        [HttpPut("/api/notification")]
        public async Task<NotificationDto> UpdateNotification([FromBody]NotificationDto notification)
        {
            return await _mediator.Send(new UpdateNotificationCommand { Notification = notification });
        }

        //[AllowAnonymous]
        //[HttpGet("/api/notification/{userId}/count")]
        //public async Task<IActionResult> NewNotificationCount(string userId)
        //{
        //    var result = _notificationService.GetNotificationCount(userId);
        //    return Ok(result);
        //}

        [HttpGet("/api/notification")]
        public async Task<NotificationDto> GetNotificationById([FromQuery]string id)
        {
            return await _mediator.Send(new GetNotificationByIdQuery { Id = id });
        }
    }
}

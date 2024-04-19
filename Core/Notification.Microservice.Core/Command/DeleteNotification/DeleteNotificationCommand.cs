using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Command.DeleteNotification;

public class DeleteNotificationCommand : IRequest<bool>
{
    public string Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
}

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, bool>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ISignalRNotificationClientWrapper _signalRNotificationClient;
    private readonly ILogger<DeleteNotificationCommandHandler> _logger;

    public DeleteNotificationCommandHandler(INotificationRepository notificationRepository,
        ISignalRNotificationClientWrapper signalRNotificationClient,
        ILogger<DeleteNotificationCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _signalRNotificationClient = signalRNotificationClient;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete operation for Notification Id: {Id} has started",request.Id);

        var result = await _notificationRepository.DeleteNotification(request.Id);

        var count = await _notificationRepository.GetNewNotificationCount(request.UserId);

        if (result)
        {
            _logger.LogInformation("Sending the NotificationCount Update to SignalR Service");

            await _signalRNotificationClient.GetCallAsync(new MenuMangement.HttpClient.Domain.Models.NotificationSignalRRequest
            {
                NotificationCount = count,
                isSendAll = false,
                FromUserId = string.Empty,
                ToUserId = request.UserId
            }, request.Token);
        }

        _logger.LogInformation("Delete operation for Notification Id: {Id} has be success: {sucess}", request.Id,result);
        return result;
    }
}

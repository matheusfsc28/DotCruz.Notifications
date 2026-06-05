using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Application.UseCases.Notifications.UpdateNotificationStatus;

public class UpdateNotificationStatusCommandHandler : IRequestHandler<UpdateNotificationStatusCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<UpdateNotificationStatusCommandHandler> _logger;

    public UpdateNotificationStatusCommandHandler(
        INotificationRepository notificationRepository,
        ILogger<UpdateNotificationStatusCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    public async Task Handle(UpdateNotificationStatusCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken);
        if (notification == null)
        {
            _logger.LogWarning(ResourceLogMessages.NOTIFICATION_NOT_FOUND_TO_UPDATE_STATUS, request.NotificationId);
            return;
        }

        if (request.Message.Success)
        {
            notification.RegisterSuccess(DateTimeOffset.UtcNow);
            _logger.LogInformation(ResourceLogMessages.NOTIFICATION_DELIVERED, request.NotificationId);
        }
        else
        {
            notification.RegisterFailure(request.Message.ErrorMessage);
            _logger.LogError(ResourceLogMessages.NOTIFICATION_DELIVERY_FAILED, request.NotificationId, request.Message.ErrorMessage);
        }

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
    }
}

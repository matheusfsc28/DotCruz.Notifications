using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Application.UseCases.Notifications.RegisterFailureNotification;

public class RegisterFailureNotificationCommandHandler : IRequestHandler<RegisterFailureNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<RegisterFailureNotificationCommandHandler> _logger;

    public RegisterFailureNotificationCommandHandler(
        INotificationRepository notificationRepository,
        ILogger<RegisterFailureNotificationCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    public async Task Handle(RegisterFailureNotificationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogCritical(ResourceLogMessages.NOTIFICATION_FAILED_DEFINITIVELY, request.NotificationId);

        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken);
        if (notification == null)
        {
            _logger.LogWarning(ResourceLogMessages.NOTIFICATION_NOT_FOUND, request.NotificationId);
            return;
        }

        notification.RegisterFailure(string.Join(" | ", request.ErrorsMessage));

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
    }
}

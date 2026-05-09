using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Application.UseCases.Notifications.PollScheduledNotifications;

public class PollScheduledNotificationsCommandHandler : IRequestHandler<PollScheduledNotificationsCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IPublishNotificationService _publishService;
    private readonly ILogger<PollScheduledNotificationsCommandHandler> _logger;

    public PollScheduledNotificationsCommandHandler(
        INotificationRepository notificationRepository,
        IPublishNotificationService publishService,
        ILogger<PollScheduledNotificationsCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _publishService = publishService;
        _logger = logger;
    }

    public async Task Handle(PollScheduledNotificationsCommand request, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetPendingScheduledAsync(DateTimeOffset.UtcNow, limit: 100, cancellationToken);

        foreach (var notification in notifications)
        {
            _logger.LogInformation(ResourceLogMessages.PROCESSING_SCHEDULED_NOTIFICATION, notification.Id);

            await _publishService.PublishNotificationCreatedEvent(notification, cancellationToken);
        }
    }
}

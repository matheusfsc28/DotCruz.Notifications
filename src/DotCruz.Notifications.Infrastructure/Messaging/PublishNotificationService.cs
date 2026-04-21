using DotCruz.Notifications.Application.Notifications.Events;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using MassTransit;

namespace DotCruz.Notifications.Infrastructure.Messaging;

public class PublishNotificationService : IPublishNotificationService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishNotificationService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        await _publishEndpoint.Publish(new NotificationCreatedEvent
        {
            NotificationId = notification.Id
        }, cancellationToken);
    }
}

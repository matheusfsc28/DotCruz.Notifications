using DotCruz.Notifications.Application.Events.Notifications;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using MassTransit;

namespace DotCruz.Notifications.Infrastructure.Services.Messaging;

public class PublishNotificationService : IPublishNotificationService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishNotificationService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishNotificationCreatedEvent(Notification notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new NotificationCreatedEvent(notification.Id), cancellationToken);
    }
}

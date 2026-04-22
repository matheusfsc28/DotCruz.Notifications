using DotCruz.Notifications.Domain.Entities.Notifications;

namespace DotCruz.Notifications.Domain.Interfaces;

public interface IPublishNotificationService
{
    Task PublishNotificationCreatedEvent(Notification notification, CancellationToken cancellationToken);
}

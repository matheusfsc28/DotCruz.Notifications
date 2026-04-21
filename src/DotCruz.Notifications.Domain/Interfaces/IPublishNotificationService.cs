using DotCruz.Notifications.Domain.Entities.Notifications;

namespace DotCruz.Notifications.Domain.Interfaces;

public interface IPublishNotificationService
{
    Task PublishAsync(Notification notification, CancellationToken cancellationToken = default);
}

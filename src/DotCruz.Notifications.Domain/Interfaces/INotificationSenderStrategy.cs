using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace DotCruz.Notifications.Domain.Interfaces;

public interface INotificationSenderStrategy
{
    NotificationType HandledType { get; }
    Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
}

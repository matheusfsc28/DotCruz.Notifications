using NotificationEntity = DotCruz.Notification.Domain.Entities.Notifications.Notification;
using DotCruz.Notification.Domain.Enums.Notifications;

namespace DotCruz.Notification.Domain.Interfaces;

public interface INotificationSenderStrategy
{
    NotificationType HandledType { get; }
    Task SendAsync(NotificationEntity notification, CancellationToken cancellationToken = default);
}

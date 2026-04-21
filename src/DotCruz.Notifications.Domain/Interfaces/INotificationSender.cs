namespace DotCruz.Notifications.Domain.Interfaces;

using DotCruz.Notifications.Domain.Entities.Notifications;

public interface INotificationSender<in TNotification> where TNotification : Notification
{
    Task SendAsync(TNotification notification, CancellationToken cancellationToken = default);
}

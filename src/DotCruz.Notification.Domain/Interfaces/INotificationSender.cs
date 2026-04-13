namespace DotCruz.Notification.Domain.Interfaces;

using DotCruz.Notification.Domain.Entities.Notifications;

public interface INotificationSender<in TNotification> where TNotification : Notification
{
    Task SendAsync(TNotification notification, CancellationToken cancellationToken = default);
}

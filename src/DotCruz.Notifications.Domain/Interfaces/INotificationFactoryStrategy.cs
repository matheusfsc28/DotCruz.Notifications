using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace DotCruz.Notifications.Domain.Interfaces;

public interface INotificationFactoryStrategy
{
    NotificationType Type { get; }
    Notification Create(
        Guid serviceId,
        string recipient,
        string? culture,
        string? body,
        string? title,
        Guid? templateId,
        Dictionary<string, object>? templateData,
        DateTimeOffset? scheduledFor);
}

using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;

namespace DotCruz.Notifications.Application.Factories.Notifications;

public class PushFactoryStrategy : INotificationFactoryStrategy
{
    public NotificationType Type => NotificationType.Push;

    public Notification Create(
        Guid serviceId,
        string recipient,
        string? culture,
        string? body,
        string? subject,
        Guid? templateId,
        Dictionary<string, object>? templateData,
        DateTimeOffset? scheduledFor)
    {
        return new PushNotification(
            serviceId,
            recipient,
            culture,
            subject ?? string.Empty,
            body,
            templateId,
            templateData,
            scheduledFor);
    }
}

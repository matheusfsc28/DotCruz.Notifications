using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;

namespace DotCruz.Notifications.Application.Factories.Notifications;

public class EmailFactoryStrategy : INotificationFactoryStrategy
{
    public NotificationType Type => NotificationType.Email;

    public Notification Create(
        Guid serviceId,
        string recipient,
        string? culture,
        string? body,
        string? title,
        Guid? templateId,
        Dictionary<string, object>? templateData,
        DateTimeOffset? scheduledFor)
    {
        return new EmailNotification(
            serviceId,
            recipient,
            culture,
            title ?? string.Empty,
            body,
            templateId,
            templateData,
            scheduledFor);
    }
}

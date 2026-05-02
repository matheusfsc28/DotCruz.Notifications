using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;

namespace DotCruz.Notifications.Application.Factories.Notifications;

public class SmsFactoryStrategy : INotificationFactoryStrategy
{
    public NotificationType Type => NotificationType.Sms;

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
        return new SmsNotification(
            serviceId,
            recipient,
            culture,
            body,
            templateId,
            templateData,
            scheduledFor);
    }
}

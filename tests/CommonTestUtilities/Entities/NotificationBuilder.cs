using CommonTestUtilities.Entities.Notifications;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Entities;

public class NotificationBuilder
{
    public static Notification Build(
        NotificationType type, 
        Guid? templateId = null,
        Dictionary<string, object>? templateData = null,
        string? body = null,
        Guid? tenantId = null)
    {
        return type switch
        {
            NotificationType.Email => EmailNotificationBuilder.Build(templateId: templateId, templateData: templateData, body: body, tenantId: tenantId),
            NotificationType.Sms => SmsNotificationBuilder.Build(templateId: templateId, templateData: templateData, body: body, tenantId: tenantId),
            NotificationType.Push => PushNotificationBuilder.Build(templateId: templateId, templateData: templateData, body: body, tenantId: tenantId),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}

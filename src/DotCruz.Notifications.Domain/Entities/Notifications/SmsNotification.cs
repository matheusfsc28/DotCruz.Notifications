using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;

namespace DotCruz.Notifications.Domain.Entities.Notifications;

public class SmsNotification : Notification
{
    public SmsNotification(
        Guid serviceId,
        string phoneNumber,
        string? culture,
        string? body,
        Guid? templateId,
        Dictionary<string, object>? templateData,
        DateTimeOffset? scheduledFor)
        : base(serviceId, NotificationType.Sms, phoneNumber, culture, body, templateId, templateData, scheduledFor)
    {
        Validate();
    }

    protected override void ValidateSpecificRules(List<string> errors) { }
}

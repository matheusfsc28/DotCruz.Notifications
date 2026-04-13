using DotCruz.Notification.Domain.Enums.Notifications;
using DotCruz.Notification.Exceptions;

namespace DotCruz.Notification.Domain.Entities.Notifications;

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

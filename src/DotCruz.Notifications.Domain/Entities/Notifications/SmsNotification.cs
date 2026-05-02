using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;

namespace DotCruz.Notifications.Domain.Entities.Notifications;

public class SmsNotification : Notification
{
    // For MongoDB
    private SmsNotification() { }

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

    public override void SetRenderedTitle(string title) { }

    protected override void ValidateSpecificRules(List<string> errors) { }
}

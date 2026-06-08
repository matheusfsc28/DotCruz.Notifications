using DotCruz.Notifications.Domain.Enums.Notifications;

namespace DotCruz.Notifications.Domain.Entities.Notifications;

public class SmsNotification : Notification
{
    private SmsNotification() { }

    public SmsNotification(
        Guid serviceId,
        string phoneNumber,
        string? culture,
        string? body,
        Guid? templateId,
        Dictionary<string, object>? templateData,
        DateTimeOffset? scheduledFor,
        Guid tenantId)
        : base(serviceId, NotificationType.Sms, phoneNumber, culture, body, templateId, templateData, scheduledFor, tenantId)
    {
        Validate();
    }

    public override void SetRenderedTitle(string title) { }

    protected override void ValidateSpecificRules(List<string> errors) { }
}

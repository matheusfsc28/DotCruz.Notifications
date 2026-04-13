using DotCruz.Notification.Domain.Enums.Notifications;
using DotCruz.Notification.Exceptions;

namespace DotCruz.Notification.Domain.Entities.Notifications;

public class EmailNotification : Notification
{
    public string Subject { get; private set; }

    public EmailNotification(
        Guid serviceId,
        string recipient,
        string? culture,
        string subject,
        string? body,
        Guid? templateId,
        Dictionary<string, object>? templateData,
        DateTimeOffset? scheduledFor)
        : base(serviceId, NotificationType.Email, recipient, culture, body, templateId, templateData, scheduledFor)
    {
        Subject = subject;
        Validate();
    }

    protected override void ValidateSpecificRules(List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(Subject))
            errors.Add(ResourceMessagesException.SUBJECT_EMPTY);
    }
}

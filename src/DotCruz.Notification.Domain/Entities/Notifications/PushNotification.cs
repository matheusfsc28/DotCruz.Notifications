using DotCruz.Notification.Domain.Enums.Notifications;
using DotCruz.Notification.Exceptions;

namespace DotCruz.Notification.Domain.Entities.Notifications;

public class PushNotification : Notification
{
    public string Title { get; private set; }

    public PushNotification(
        Guid serviceId,
        string deviceToken,
        string? culture,
        string title,
        string? body,
        Guid? templateId,
        Dictionary<string, object>? templateData,
        DateTimeOffset? scheduledFor)
        : base(serviceId, NotificationType.Push, deviceToken, culture, body, templateId, templateData, scheduledFor)
    {
        Title = title;
        Validate();
    }

    protected override void ValidateSpecificRules(List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(Title))
            errors.Add(ResourceMessagesException.SUBJECT_EMPTY);
    }
}

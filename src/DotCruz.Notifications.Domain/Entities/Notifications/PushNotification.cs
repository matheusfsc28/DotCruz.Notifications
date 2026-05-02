using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;

namespace DotCruz.Notifications.Domain.Entities.Notifications;

public class PushNotification : Notification
{
    public string Title { get; private set; } = string.Empty;

    // For MongoDB
    private PushNotification() { }

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

    public override void SetRenderedTitle(string title)
    {
        Title = title;
        Validate();
    }

    protected override void ValidateSpecificRules(List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(Title))
            errors.Add(ResourceMessagesException.TITLE_EMPTY);
    }
}

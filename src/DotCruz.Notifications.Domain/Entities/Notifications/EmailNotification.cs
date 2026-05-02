using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;

namespace DotCruz.Notifications.Domain.Entities.Notifications;

public class EmailNotification : Notification
{
    public string Title { get; private set; } = string.Empty;

    private EmailNotification() { }

    public EmailNotification(
        Guid serviceId,
        string recipient,
        string? culture,
        string title,
        string? body,
        Guid? templateId,
        Dictionary<string, object>? templateData,
        DateTimeOffset? scheduledFor)
        : base(serviceId, NotificationType.Email, recipient, culture, body, templateId, templateData, scheduledFor)
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
        if (string.IsNullOrWhiteSpace(Title) && TemplateId == null)
            errors.Add(ResourceMessagesException.TITLE_EMPTY);
    }
}

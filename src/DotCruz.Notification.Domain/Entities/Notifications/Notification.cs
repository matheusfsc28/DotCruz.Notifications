using DotCruz.Notification.Domain.Entities.Base;
using DotCruz.Notification.Domain.Enums.Notifications;
using DotCruz.Notification.Domain.ValueObjects.Notifications;
using DotCruz.Notification.Exceptions;
using DotCruz.Notification.Exceptions.BaseExceptions;
using System.Globalization;

namespace DotCruz.Notification.Domain.Entities.Notifications;

public abstract class Notification : BaseEntity
{
    public Guid ServiceId { get; private set; }
    public string? CallerReferenceId { get; private set; }
    public string? Context { get; private set; }
    public NotificationType Type { get; private set; }
    public string Culture { get; private set; } = "pt-BR";
    public string Recipient { get; private set; } = string.Empty;
    public string? Body { get; private set; }
    public Guid? TemplateId { get; private set; }
    public Dictionary<string, object>? TemplateData { get; private set; }
    public NotificationStatus Status { get; private set; } = NotificationStatus.Pending;
    public DateTimeOffset? ScheduledFor { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public int RetryCount { get; private set; } = 0;

    private readonly List<NotificationError> _errors = [];
    public IReadOnlyCollection<NotificationError> Errors => _errors.AsReadOnly();

    protected bool HasBody => !string.IsNullOrWhiteSpace(Body);
    protected bool HasTemplate => TemplateId.GetValueOrDefault() != Guid.Empty;

    protected Notification(Guid serviceId, NotificationType type, string recipient, string? culture, string? body, Guid? templateId, Dictionary<string, object>? templateData, DateTimeOffset? scheduledFor)
    {
        ServiceId = serviceId;
        Type = type;
        Culture = !string.IsNullOrEmpty(culture) ? culture : Culture;
        Recipient = recipient;
        Body = body;
        TemplateId = templateId;
        TemplateData = templateData;
        ScheduledFor = scheduledFor;
    }

    public void RegisterSuccess(DateTimeOffset sentAt)
    {
        Status = NotificationStatus.Sent;
        SentAt = sentAt;
        Validate();
    }

    public void RegisterFailure(string? errorMessage)
    {
        var errorDetail = !string.IsNullOrEmpty(errorMessage) ? errorMessage : ResourceMessagesException.UNKNOWN_ERROR;
        _errors.Add(new NotificationError(errorDetail));

        RetryCount++;

        Validate();
    }

    protected virtual void Validate()
    {
        var errors = new List<string>();

        if (ServiceId == Guid.Empty)
            errors.Add(ResourceMessagesException.SERVICE_ID_EMPTY);

        if (string.IsNullOrEmpty(Recipient))
            errors.Add(ResourceMessagesException.RECIPIENT_EMPTY);

        if (string.IsNullOrEmpty(Culture))
            errors.Add(ResourceMessagesException.CULTURE_EMPTY);

        if (!HasBody && !HasTemplate)
            errors.Add(ResourceMessagesException.BODY_OR_TEMPLATE_REQUIRED);

        if (Status == NotificationStatus.Sent && SentAt == null)
            errors.Add(ResourceMessagesException.SENT_DATE_EMPTY);

        ValidateSpecificRules(errors);

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }

    protected abstract void ValidateSpecificRules(List<string> errors);
}

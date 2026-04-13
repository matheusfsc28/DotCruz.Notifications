using DotCruz.Notification.Exceptions;
using DotCruz.Notification.Exceptions.BaseExceptions;

namespace DotCruz.Notification.Domain.ValueObjects.Notifications;

public record NotificationError
{
    public string Message { get; init; }
    public DateTimeOffset OccurredAt { get; init; }

    public NotificationError(string message)
    {
        Message = message;
        OccurredAt = DateTimeOffset.UtcNow;

        Validate();
    }

    private void Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(Message))
            errors.Add(ResourceMessagesException.ERROR_EMPTY);

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }
}

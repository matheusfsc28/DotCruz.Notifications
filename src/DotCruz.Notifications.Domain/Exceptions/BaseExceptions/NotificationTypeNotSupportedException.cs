using DotCruz.Notifications.Domain.Exceptions.Enums;
using DotCruz.Notifications.Domain.Exceptions.Resources;

namespace DotCruz.Notifications.Domain.Exceptions.BaseExceptions;

public class NotificationTypeNotSupportedException : NotificationException
{
    public NotificationTypeNotSupportedException() : base(ResourceMessagesException.NOTIFICATION_TYPE_NOT_SUPPORTED)
    {
    }

    public override IEnumerable<string> GetErrorsMessages() => [Message];

    public override ErrorType GetErrorType() => ErrorType.Validation;
}

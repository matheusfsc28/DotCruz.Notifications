using DotCruz.Notifications.Domain.Exceptions.Enums;

namespace DotCruz.Notifications.Domain.Exceptions.BaseExceptions;

public class NotFoundException : NotificationException
{
    public NotFoundException(string message) : base(message) { }

    public override IEnumerable<string> GetErrorsMessages() => [Message];

    public override ErrorType GetErrorType() => ErrorType.NotFound;
}

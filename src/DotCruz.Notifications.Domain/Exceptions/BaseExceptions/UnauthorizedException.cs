using DotCruz.Notifications.Domain.Exceptions.Enums;

namespace DotCruz.Notifications.Domain.Exceptions.BaseExceptions;

public class UnauthorizedException : NotificationException
{
    public UnauthorizedException(string message) : base(message) { }

    public override IEnumerable<string> GetErrorsMessages() => [Message];

    public override ErrorType GetErrorType() => ErrorType.Unauthorized;
}

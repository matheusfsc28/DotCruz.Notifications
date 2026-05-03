using DotCruz.Notifications.Exceptions.Enums;

namespace DotCruz.Notifications.Exceptions.BaseExceptions;

public class UnauthorizedException : NotificationException
{
    public UnauthorizedException(string message) : base(message) { }

    public override IEnumerable<string> GetErrorsMessages() => [Message];

    public override ErrorType GetErrorType() => ErrorType.Unauthorized;
}

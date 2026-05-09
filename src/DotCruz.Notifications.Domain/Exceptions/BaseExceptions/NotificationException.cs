using DotCruz.Notifications.Domain.Exceptions.Enums;

namespace DotCruz.Notifications.Domain.Exceptions.BaseExceptions;

public abstract class NotificationException : Exception
{
    protected NotificationException(string message) : base(message) { }
    public abstract IEnumerable<string> GetErrorsMessages();
    public abstract ErrorType GetErrorType();
}

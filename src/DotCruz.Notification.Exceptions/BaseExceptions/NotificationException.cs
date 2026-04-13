using DotCruz.Notification.Exceptions.Enums;
using System.Net;

namespace DotCruz.Notification.Exceptions.BaseExceptions;

public abstract class NotificationException : Exception
{
    protected NotificationException(string message) : base(message) { }
    public abstract IEnumerable<string> GetErrorsMessages();
    public abstract ErrorType GetErrorType();
}

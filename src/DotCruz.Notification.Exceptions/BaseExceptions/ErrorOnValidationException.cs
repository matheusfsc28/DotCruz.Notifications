using DotCruz.Notification.Exceptions.Enums;
using System.Net;

namespace DotCruz.Notification.Exceptions.BaseExceptions;

public class ErrorOnValidationException : NotificationException
{
    public IEnumerable<string> Errors { get; private set; }

    public ErrorOnValidationException(IEnumerable<string> errors) : base(string.Empty) => Errors = errors;
    public ErrorOnValidationException(string error) : base(string.Empty) => Errors = [error];

    public override IEnumerable<string> GetErrorsMessages() => Errors;

    public override ErrorType GetErrorType() => ErrorType.Validation;
}

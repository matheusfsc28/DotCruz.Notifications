using DotCruz.Notifications.Domain.Exceptions.Enums;

namespace DotCruz.Notifications.Api.Extensions;

public static class ErrorTypeExtensions
{
    public static int MapToStatusCode(this ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        _ => StatusCodes.Status500InternalServerError,
    };
}

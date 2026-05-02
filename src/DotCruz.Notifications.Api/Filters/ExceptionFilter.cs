using DotCruz.Notifications.Application.DTOs.Base;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;
using DotCruz.Notifications.Exceptions.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotCruz.Notifications.Api.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NotificationException exception)
        {
            HandleProjectException(exception, context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleProjectException(NotificationException exception, ExceptionContext context)
    {
        var statusCode = MapErrorType(exception.GetErrorType());
        var errors = exception.GetErrorsMessages();

        logger.LogWarning(
            context.Exception,
            "Handled business exception. Status: {StatusCode}. Errors: {@Errors}",
            statusCode,
            errors);

        context.HttpContext.Response.StatusCode = statusCode;
        context.Result = new ObjectResult(new ErrorResponseDto(errors));

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        logger.LogError(
            context.Exception,
            "Unhandled exception caught in Controller: {ControllerName}, Action: {ActionName}, Error: {Message}",
            context.ActionDescriptor.DisplayName,
            context.HttpContext.Request.Method,
            context.Exception.Message);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ErrorResponseDto(ResourceMessagesException.UNKNOWN_ERROR));

        context.ExceptionHandled = true;
    }

    private static int MapErrorType(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError,
    };
}
using DotCruz.Notifications.Api.Extensions;
using DotCruz.Notifications.Application.DTOs.Base;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace DotCruz.Notifications.Api.Filters;

public class AuthenticatedServiceFilter : IAsyncAuthorizationFilter
{
    private const string APIKEYNAME = "X-Api-Key";

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (IsAnonymousAuthorization(context))
            return;

        try
        {
            var token = TokenOnRequest(context);
            var apiKey = GetApiKeySettings(context);

            if (!apiKey.Equals(token))
                throw new UnauthorizedException(ResourceMessagesException.TOKEN_INVALID);
        }
        catch(NotificationException notificationException)
        {
            context.HttpContext.Response.StatusCode = notificationException.GetErrorType().MapToStatusCode();
            context.Result = new ObjectResult(new ErrorResponseDto(notificationException.GetErrorsMessages()));
        }
        catch(Exception)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ErrorResponseDto((ResourceMessagesException.UNKNOWN_ERROR)));
        }
    }

    private static bool IsAnonymousAuthorization(AuthorizationFilterContext context)
        => context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);

        return extractedApiKey!;
    }

    private static string GetApiKeySettings(AuthorizationFilterContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = configuration.GetValue<string>("Settings:ApiKey");

        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException("The api key is missing on env values");

        return apiKey;
    }
}

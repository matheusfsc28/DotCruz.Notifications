using System.Globalization;

namespace DotCruz.Notifications.Api.Middlewares;

public class CultureMiddleware
{
    private static readonly HashSet<string> _supportedCultureNames =
    new(["en", "pt-BR"], StringComparer.OrdinalIgnoreCase);

    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        var cultureInfo = new CultureInfo("en");

        if (requestedCulture != null && requestedCulture.Length != 0 && _supportedCultureNames.Contains(requestedCulture!))
        {
            cultureInfo = new CultureInfo(requestedCulture!);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}

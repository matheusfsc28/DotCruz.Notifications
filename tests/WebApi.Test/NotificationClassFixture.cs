using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class NotificationClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public NotificationClassFixture(CustomWebApplicationFactory factory)
        => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(
        string endpoint,
        object request,
        string token = "",
        string culture = "en"
    )
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PostAsJsonAsync(endpoint, request);
    }

    protected async Task<HttpResponseMessage> DoPut(string method, object request, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PutAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoDelete(string method, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.DeleteAsync(method);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }

    private void AuthorizeRequest(string token)
    {
        _httpClient.DefaultRequestHeaders.Remove("X-Api-Key");

        if (string.IsNullOrWhiteSpace(token))
            return;

        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", token);
    }
}
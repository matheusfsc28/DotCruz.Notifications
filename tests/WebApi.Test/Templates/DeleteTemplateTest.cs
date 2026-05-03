using CommonTestUtilities.InlineData;
using DotCruz.Notifications.Application.DTOs.Base;
using DotCruz.Notifications.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Templates;

public class DeleteTemplateTest : NotificationClassFixture
{
    private readonly string ENDPOINT = "api/template";
    private readonly string _apiToken;
    private readonly Guid _templateId;

    public DeleteTemplateTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _apiToken = factory.GetApiToken();
        _templateId = factory.GetTemplateId();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoDelete($"{ENDPOINT}/{_templateId}", _apiToken);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized_Request(string culture)
    {
        var response = await DoDelete($"{ENDPOINT}/{_templateId}", string.Empty, culture: culture);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        var response = await DoDelete($"{ENDPOINT}/{_templateId}", token: "invalidToken", culture: culture);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOKEN_INVALID", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Template_Not_Found(string culture)
    {
        var response = await DoDelete($"{ENDPOINT}/{Guid.NewGuid()}", token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TEMPLATE_NOT_FOUND", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }
}

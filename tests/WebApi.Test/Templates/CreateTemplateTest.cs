using CommonTestUtilities.Commands.Templates;
using CommonTestUtilities.InlineData;
using DotCruz.Notifications.Application.DTOs.Base;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Templates;

public class CreateTemplateTest : NotificationClassFixture
{
    private readonly string ENDPOINT = "api/template";
    private readonly string _apiToken;

    public CreateTemplateTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _apiToken = factory.GetApiToken();
    }

    [Theory]
    [ClassData(typeof(NotificationTypeInlineDataTest))]
    public async Task Success(NotificationType type)
    {
        var request = CreateTemplateCommandBuilder.Build(type: type);

        var response = await DoPost(ENDPOINT, request, _apiToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var responseId = await response.Content.ReadFromJsonAsync<Guid>();

        Assert.NotEqual(Guid.Empty, responseId);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized_Request(string culture)
    {
        var request = CreateTemplateCommandBuilder.Build();

        var response = await DoPost(ENDPOINT, request, culture: culture);

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
        var request = CreateTemplateCommandBuilder.Build();

        var response = await DoPost(ENDPOINT, request, token: "invalidToken", culture: culture);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOKEN_INVALID", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Code(string culture)
    {
        var request = CreateTemplateCommandBuilder.Build(code: string.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("CODE_EMPTY", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Culture(string culture)
    {
        var request = CreateTemplateCommandBuilder.Build(culture: string.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("CULTURE_EMPTY", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_DefaultTitle(string culture)
    {
        var request = CreateTemplateCommandBuilder.Build(defaultTitle: string.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("DEFAULT_TITLE_EMPTY", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Body(string culture)
    {
        var request = CreateTemplateCommandBuilder.Build(body: string.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("BODY_EMPTY", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_NotificationType(string culture)
    {
        var request = CreateTemplateCommandBuilder.Build(type: (NotificationType)100);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NOTIFICATION_TYPE_INVALID", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Template_Already_Exists(string culture)
    {
        var request = CreateTemplateCommandBuilder.Build();

        await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TEMPLATE_ALREADY_EXISTS", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }
}

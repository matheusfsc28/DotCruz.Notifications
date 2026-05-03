using CommonTestUtilities.Commands.Templates;
using CommonTestUtilities.InlineData;
using DotCruz.Notifications.Application.DTOs.Base;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Templates;

public class UpdateTemplateTest : NotificationClassFixture
{
    private readonly string ENDPOINT = "api/template";
    private readonly string _apiToken;
    private readonly Guid _templateId;

    public UpdateTemplateTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _apiToken = factory.GetApiToken();
        _templateId = factory.GetTemplateId();
    }

    [Theory]
    [ClassData(typeof(NotificationTypeInlineDataTest))]
    public async Task Success(NotificationType type)
    {
        var request = UpdateTemplateCommandBuilder.Build(type: type);

        var response = await DoPut($"{ENDPOINT}/{_templateId}", request, _apiToken);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized_Request(string culture)
    {
        var request = UpdateTemplateCommandBuilder.Build();

        var response = await DoPut($"{ENDPOINT}/{_templateId}", request, string.Empty, culture: culture);

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
        var request = UpdateTemplateCommandBuilder.Build();

        var response = await DoPut($"{ENDPOINT}/{_templateId}", request, token: "invalidToken", culture: culture);

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
        var request = UpdateTemplateCommandBuilder.Build();

        var response = await DoPut($"{ENDPOINT}/{Guid.NewGuid()}", request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TEMPLATE_NOT_FOUND", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_NotificationType(string culture)
    {
        var request = UpdateTemplateCommandBuilder.Build(type: (NotificationType)100);

        var response = await DoPut($"{ENDPOINT}/{_templateId}", request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NOTIFICATION_TYPE_INVALID", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }
}

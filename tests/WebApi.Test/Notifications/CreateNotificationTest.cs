using CommonTestUtilities.Commands.Notifications;
using CommonTestUtilities.InlineData;
using DotCruz.Notifications.Application.DTOs.Base;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Notifications;

public class CreateNotificationTest : NotificationClassFixture
{
    private readonly string ENDPOINT = "api/notification";
    private readonly string _apiToken;
    private readonly Guid _templateId;

    public CreateNotificationTest(CustomWebApplicationFactory factory) : base(factory) 
    {
        _apiToken = factory.GetApiToken();
        _templateId = factory.GetTemplateId();
    }

    [Theory]
    [ClassData(typeof(NotificationTypeInlineDataTest))]
    public async Task Success(NotificationType type)
    {
        var request = CreateNotificationCommandBuilder.Build(type: type);

        var response = await DoPost(ENDPOINT, request, _apiToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var responseId = await response.Content.ReadFromJsonAsync<Guid>();

        Assert.NotEqual(Guid.Empty, responseId);
    }

    [Theory]
    [ClassData(typeof(NotificationTypeInlineDataTest))]
    public async Task Success_With_Template(NotificationType type)
    {
        var request = CreateNotificationCommandBuilder.Build(type: type, templateId: _templateId);

        var response = await DoPost(ENDPOINT, request, _apiToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var responseId = await response.Content.ReadFromJsonAsync<Guid>();

        Assert.NotEqual(Guid.Empty, responseId);
    }

    [Theory]
    [ClassData(typeof(NotificationTypeInlineDataTest))]
    public async Task Success_Scheduled(NotificationType type)
    {
        var request = CreateNotificationCommandBuilder.Build(type: type, scheduledFor: DateTimeOffset.UtcNow.AddDays(1));

        var response = await DoPost(ENDPOINT, request, _apiToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var responseId = await response.Content.ReadFromJsonAsync<Guid>();

        Assert.NotEqual(Guid.Empty, responseId);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized_Request(string culture)
    {
        var request = CreateNotificationCommandBuilder.Build();

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
        var request = CreateNotificationCommandBuilder.Build();

        var response = await DoPost(ENDPOINT, request, token: "invalidToken", culture: culture);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOKEN_INVALID", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Recipient(string culture)
    {
        var request = CreateNotificationCommandBuilder.Build(recipient: string.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPIENT_EMPTY", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_ServiceId(string culture)
    {
        var request = CreateNotificationCommandBuilder.Build(serviceId: Guid.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("SERVICE_ID_EMPTY", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Body_And_Template_Empty(string culture)
    {
        var request = CreateNotificationCommandBuilder.Build(body: string.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("BODY_OR_TEMPLATE_REQUIRED", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_NotificationType(string culture)
    {
        var request = CreateNotificationCommandBuilder.Build(type: (NotificationType)100);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NOTIFICATION_TYPE_INVALID", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Title_Email(string culture)
    {
        var request = CreateNotificationCommandBuilder.Build(type: NotificationType.Email, title: string.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Title_Push(string culture)
    {
        var request = CreateNotificationCommandBuilder.Build(type: NotificationType.Push, title: string.Empty);

        var response = await DoPost(ENDPOINT, request, token: _apiToken, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture));

        Assert.NotNull(responseData);
        Assert.Contains(expectedMessage, responseData.Errors);
    }
}

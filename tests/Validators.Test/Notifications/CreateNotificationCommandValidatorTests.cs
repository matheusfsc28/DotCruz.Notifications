using CommonTestUtilities.Commands.Notifications;
using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;

namespace Validators.Test.Notifications;

public class CreateNotificationCommandValidatorTests
{
    [Fact]
    public async Task Success()
    {
        var validator = new CreateNotificationCommandValidator();
        var request = CreateNotificationCommandBuilder.Build();

        var result = await validator.ValidateAsync(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Success_With_TemplateId()
    {
        var validator = new CreateNotificationCommandValidator();
        var request = CreateNotificationCommandBuilder.Build(body: string.Empty, templateId: Guid.NewGuid());

        var result = await validator.ValidateAsync(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Error_ServiceId_Empty()
    {
        var validator = new CreateNotificationCommandValidator();
        var request = CreateNotificationCommandBuilder.Build(serviceId: Guid.Empty);

        var result = await validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.SERVICE_ID_EMPTY));
    }

    [Fact]
    public async Task Error_Recipient_Empty()
    {
        var validator = new CreateNotificationCommandValidator();
        var request = CreateNotificationCommandBuilder.Build(recipient: string.Empty);

        var result = await validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.RECIPIENT_EMPTY));
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var validator = new CreateNotificationCommandValidator();
        var request = CreateNotificationCommandBuilder.Build(type: (NotificationType)999);

        var result = await validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.NOTIFICATION_TYPE_INVALID));
    }

    [Fact]
    public async Task Error_Body_And_Template_Empty()
    {
        var validator = new CreateNotificationCommandValidator();
        var request = CreateNotificationCommandBuilder.Build(body: string.Empty, templateId: null);

        var result = await validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.BODY_OR_TEMPLATE_REQUIRED));
    }
}
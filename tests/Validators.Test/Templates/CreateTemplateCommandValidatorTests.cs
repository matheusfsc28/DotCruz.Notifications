using CommonTestUtilities.Commands.Templates;
using DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Exceptions;

namespace Validators.Test.Templates;

public class CreateTemplateCommandValidatorTests
{
    [Fact]
    public async Task Success()
    {
        var validator = new CreateTemplateCommandValidator();
        var request = CreateTemplateCommandBuilder.Build();

        var result = await validator.ValidateAsync(request, TestContext.Current.CancellationToken);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Error_Code_Empty()
    {
        var validator = new CreateTemplateCommandValidator();
        var request = CreateTemplateCommandBuilder.Build();
        request = request with { Code = string.Empty };

        var result = await validator.ValidateAsync(request, TestContext.Current.CancellationToken);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.CODE_EMPTY));
    }

    [Fact]
    public async Task Error_Culture_Empty()
    {
        var validator = new CreateTemplateCommandValidator();
        var request = CreateTemplateCommandBuilder.Build();
        request = request with { Culture = string.Empty };

        var result = await validator.ValidateAsync(request, TestContext.Current.CancellationToken);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.CULTURE_EMPTY));
    }

    [Fact]
    public async Task Error_DefaultSubject_Empty()
    {
        var validator = new CreateTemplateCommandValidator();
        var request = CreateTemplateCommandBuilder.Build();
        request = request with { DefaultSubject = string.Empty };

        var result = await validator.ValidateAsync(request, TestContext.Current.CancellationToken);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.DEFAULT_SUBJECT_EMPTY));
    }

    [Fact]
    public async Task Error_Body_Empty()
    {
        var validator = new CreateTemplateCommandValidator();
        var request = CreateTemplateCommandBuilder.Build();
        request = request with { Body = string.Empty };

        var result = await validator.ValidateAsync(request, TestContext.Current.CancellationToken);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.BODY_EMPTY));
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var validator = new CreateTemplateCommandValidator();
        var request = CreateTemplateCommandBuilder.Build(type: (NotificationType)999);

        var result = await validator.ValidateAsync(request, TestContext.Current.CancellationToken);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceMessagesException.NOTIFICATION_TYPE_INVALID));
    }
}

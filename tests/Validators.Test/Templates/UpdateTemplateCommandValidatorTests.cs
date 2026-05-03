using DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;
using DotCruz.Notifications.Domain.Enums.Notifications;
using FluentValidation.TestHelper;

namespace Validators.Test.Templates;

public class UpdateTemplateCommandValidatorTest
{
    private readonly UpdateTemplateCommandValidator _validator = new();

    [Fact]
    public void Error_Invalid_Type()
    {
        var command = new UpdateTemplateCommand(
            Id: Guid.NewGuid(),
            Code: "CODE",
            Culture: "pt-BR",
            DefaultTitle: "TITLE",
            Body: "BODY",
            Type: (NotificationType)100);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Type);
    }

    [Fact]
    public void Success_Null_Type()
    {
        var command = new UpdateTemplateCommand(
            Id: Guid.NewGuid(),
            Code: "CODE",
            Culture: "pt-BR",
            DefaultTitle: "TITLE",
            Body: "BODY",
            Type: null);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}

using DotCruz.Notifications.Exceptions;
using FluentValidation;

namespace DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;

public class UpdateTemplateCommandValidator : AbstractValidator<UpdateTemplateCommand>
{
    public UpdateTemplateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Type)
            .IsInEnum()
            .When(x => x.Type.HasValue)
            .WithMessage(ResourceMessagesException.NOTIFICATION_TYPE_INVALID);
    }
}

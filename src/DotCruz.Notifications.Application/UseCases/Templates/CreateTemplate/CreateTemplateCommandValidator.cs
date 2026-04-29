using DotCruz.Notifications.Exceptions;
using FluentValidation;

namespace DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;

public class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
{
    public CreateTemplateCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.CODE_EMPTY);

        RuleFor(x => x.Culture)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.CULTURE_EMPTY);

        RuleFor(x => x.DefaultSubject)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.DEFAULT_SUBJECT_EMPTY);

        RuleFor(x => x.Body)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.BODY_EMPTY);

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.NOTIFICATION_TYPE_INVALID);
    }
}

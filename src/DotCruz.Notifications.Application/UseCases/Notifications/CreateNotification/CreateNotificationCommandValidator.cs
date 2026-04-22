using FluentValidation;
using DotCruz.Notifications.Exceptions;

namespace DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(x => x.ServiceId)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.SERVICE_ID_EMPTY);

        RuleFor(x => x.Recipient)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.RECIPIENT_EMPTY);

        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Body) || x.TemplateId.HasValue)
            .WithMessage(ResourceMessagesException.BODY_OR_TEMPLATE_REQUIRED);
    }
}

using DotCruz.Notifications.Domain.Enums.Notifications;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;

public record CreateTemplateCommand(
    string Code,
    string Culture,
    string DefaultTitle,
    string Body,
    NotificationType Type) : IRequest<Guid>;

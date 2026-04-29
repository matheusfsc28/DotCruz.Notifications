using DotCruz.Notifications.Domain.Enums.Notifications;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;

public record UpdateTemplateCommand(
    Guid Id,
    string? Code,
    string? Culture,
    string? DefaultSubject,
    string? Body,
    NotificationType? Type) : IRequest;

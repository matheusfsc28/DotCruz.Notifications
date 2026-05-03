using System.Text.Json.Serialization;
using DotCruz.Notifications.Domain.Enums.Notifications;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;

public record UpdateTemplateCommand(
    [property: JsonIgnore] Guid Id,
    string? Code,
    string? Culture,
    string? DefaultTitle,
    string? Body,
    NotificationType? Type) : IRequest<Unit>;

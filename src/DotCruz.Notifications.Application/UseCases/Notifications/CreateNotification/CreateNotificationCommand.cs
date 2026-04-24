using DotCruz.Notifications.Domain.Enums.Notifications;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;

public record CreateNotificationCommand(
    Guid ServiceId,
    NotificationType Type,
    string Recipient,
    string? Culture = null,
    string? Body = null,
    string? Subject = null,
    Guid? TemplateId = null,
    Dictionary<string, object>? TemplateData = null,
    DateTimeOffset? ScheduledFor = null
) : IRequest<Guid>;

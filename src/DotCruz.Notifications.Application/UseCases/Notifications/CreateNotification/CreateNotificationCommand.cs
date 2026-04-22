using DotCruz.Notifications.Domain.Enums.Notifications;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;

public record CreateNotificationCommand : IRequest<Guid>
{
    public Guid ServiceId { get; init; }
    public NotificationType Type { get; init; }
    public string Recipient { get; init; } = string.Empty;
    public string? Culture { get; init; }
    public string? Body { get; init; }
    public string? Subject { get; init; }
    public Guid? TemplateId { get; init; }
    public Dictionary<string, object>? TemplateData { get; init; }
    public DateTimeOffset? ScheduledFor { get; init; }
}

namespace DotCruz.Notifications.Application.Notifications.Events;

public record NotificationCreatedEvent
{
    public Guid NotificationId { get; init; }
}

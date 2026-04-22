namespace DotCruz.Notifications.Application.Events.Notifications;

public record NotificationCreatedEvent
{
    public Guid NotificationId { get; init; }
}

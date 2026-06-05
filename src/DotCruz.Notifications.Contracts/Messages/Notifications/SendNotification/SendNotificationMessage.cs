using DotCruz.Notifications.Contracts.Enums.Notifications;

namespace DotCruz.Notifications.Contracts.Messages.Notifications.SendNotification;

public class SendNotificationMessage
{
    public Guid NotificationId { get; init; }
    public IntegrationNotificationType NotificationType { get; init; }
    public string Recipient { get; init; }
    public string? Title { get; init; }
    public string Body { get; init; }

    public SendNotificationMessage(Guid notificationId, IntegrationNotificationType notificationType, string recipient, string body, string? title = null)
    {
        NotificationId = notificationId;
        NotificationType = notificationType;
        Recipient = recipient;
        Body = body;
        Title = title;
    }
}

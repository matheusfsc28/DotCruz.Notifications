using CommonTestUtilities.Entities.Notifications;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Entities;

public class NotificationBuilder
{
    public static Notification Build(NotificationType type)
    {
        return type switch
        {
            NotificationType.Email => EmailNotificationBuilder.Build(),
            NotificationType.Sms => SmsNotificationBuilder.Build(),
            NotificationType.Push => PushNotificationBuilder.Build(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
using Bogus;
using DotCruz.Notification.Domain.Entities.Notifications;

namespace CommonTestUtilities.Entities.Notifications;

public class EmailNotificationBuilder
{
    public static EmailNotification Build(
        Guid? serviceId = null,
        string? recipient = null,
        string? culture = null,
        string? subject = null,
        string? body = null,
        Guid? templateId = null,
        Dictionary<string, object>? templateData = null,
        DateTimeOffset? scheduledFor = null)
    {
        var f = new Faker();

        if (body is null && templateId is null)
            body = f.Lorem.Paragraph();

        return new EmailNotification(
            serviceId: serviceId ?? f.Random.Guid(),
            recipient: recipient ?? f.Internet.Email(),
            culture: culture ?? "pt-BR",
            subject: subject ?? f.Lorem.Sentence(),
            body: body,
            templateId: templateId,
            templateData: templateData,
            scheduledFor: scheduledFor ?? f.Date.FutureOffset()
        );
    }
}

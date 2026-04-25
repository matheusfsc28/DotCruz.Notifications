using Bogus;
using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Commands.Notifications;

public class CreateNotificationCommandBuilder
{
    public static CreateNotificationCommand Build(
        Guid? serviceId = null,
        NotificationType? type = null,
        string? recipient = null,
        string? culture = null,
        string? body = null,
        string? subject = null,
        Guid? templateId = null,
        Dictionary<string, object>? templateData = null,
        DateTimeOffset? scheduledFor = null)
    {
        var f = new Faker();

        return new CreateNotificationCommand(
            ServiceId: serviceId ?? f.Random.Guid(),
            Type: type ?? f.PickRandom<NotificationType>(),
            Recipient: recipient ?? f.Internet.Email(),
            Culture: culture,
            Body: body ?? (templateId.HasValue ? null : f.Lorem.Paragraph()),
            Subject: subject ?? f.Lorem.Sentence(),
            TemplateId: templateId,
            TemplateData: templateData,
            ScheduledFor: scheduledFor
        );
    }
}

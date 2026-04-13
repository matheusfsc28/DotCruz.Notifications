using Bogus;
using DotCruz.Notification.Domain.Entities.Notifications;

namespace CommonTestUtilities.Entities.Notifications;

public class PushNotificationBuilder
{
    public static PushNotification Build(
        Guid? serviceId = null,
        string? deviceToken = null,
        string? culture = null,
        string? title = null,
        string? body = null,
        Guid? templateId = null,
        Dictionary<string, object>? templateData = null,
        DateTimeOffset? scheduledFor = null)
    {
        var faker = new Faker<PushNotification>()
            .CustomInstantiator(f => new PushNotification(
                    serviceId: serviceId ?? f.Random.Guid(),
                    deviceToken: deviceToken ?? f.Random.Guid().ToString(),
                    culture: culture ?? f.PickRandom("pt-BR", "en-US", "es-ES"),
                    title: title ?? f.Lorem.Sentence(),
                    body: body ?? f.Lorem.Paragraph(),
                    templateId: templateId,
                    templateData: templateData,
                    scheduledFor: scheduledFor ?? f.Date.FutureOffset()
                )
            );

        return faker.Generate();
    }
}

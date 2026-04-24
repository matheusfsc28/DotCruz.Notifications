using Bogus;
using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Commands.Notifications;

public class CreateNotificationCommandBuilder
{
    public static CreateNotificationCommand Build(NotificationType? type = null)
    {
        return new Faker<CreateNotificationCommand>()
            .CustomInstantiator(f => new CreateNotificationCommand(
                    f.Random.Guid(),
                    type ?? f.PickRandom<NotificationType>(),
                    f.Person.Email,
                    null,
                    f.Lorem.Paragraph(),
                    f.Lorem.Sentence()
                )
            )
            .Generate();
    }
}

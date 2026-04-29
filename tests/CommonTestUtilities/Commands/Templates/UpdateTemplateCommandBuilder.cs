using Bogus;
using DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Commands.Templates;

public class UpdateTemplateCommandBuilder
{
    public static UpdateTemplateCommand Build(Guid? id = null)
    {
        return new Faker<UpdateTemplateCommand>()
            .CustomInstantiator(f => new UpdateTemplateCommand(
                id ?? Guid.NewGuid(),
                f.Random.Word(),
                "en-US",
                f.Lorem.Sentence(),
                f.Lorem.Paragraph(),
                f.PickRandom<NotificationType>()))
            .Generate();
    }
}

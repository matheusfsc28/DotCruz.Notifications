using Bogus;
using DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Commands.Templates;

public class CreateTemplateCommandBuilder
{
    public static CreateTemplateCommand Build(NotificationType? type = null)
    {
        return new Faker<CreateTemplateCommand>()
            .CustomInstantiator(f => new CreateTemplateCommand(
                f.Random.Word(),
                "pt-BR",
                f.Lorem.Sentence(),
                f.Lorem.Paragraph(),
                type ?? f.PickRandom<NotificationType>()))
            .Generate();
    }
}

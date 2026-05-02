using Bogus;
using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Entities.Templates;

public class TemplateBuilder
{
    public static Template Build(
        string? code = null,
        string? culture = null,
        string? defaultTitle = null,
        string? body = null,
        NotificationType? type = null)
    {
        var faker = new Faker<Template>()
            .CustomInstantiator(f => new Template(
                    code: code ?? f.Lorem.Slug(),
                    culture: culture ?? f.PickRandom("pt-BR", "en-US", "es-ES"),
                    defaultTitle: defaultTitle ?? f.Lorem.Sentence(),
                    body: body ?? f.Lorem.Paragraph(),
                    type: type ?? f.PickRandom<NotificationType>()
                )
            );

        return faker.Generate();
    }
}

using Bogus;
using DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Commands.Templates;

public class CreateTemplateCommandBuilder
{
    public static CreateTemplateCommand Build(
        string? code = null,
        string? culture = null,
        string? defaultTitle = null,
        string? body = null,
        NotificationType? type = null)
    {
        var f = new Faker();

        return new CreateTemplateCommand(
            Code: code ?? f.Random.Word(),
            Culture: culture ?? "pt-BR",
            DefaultTitle: defaultTitle ?? f.Lorem.Sentence(),
            Body: body ?? f.Lorem.Paragraph(),
            Type: type ?? f.PickRandom<NotificationType>()
        );
    }
}

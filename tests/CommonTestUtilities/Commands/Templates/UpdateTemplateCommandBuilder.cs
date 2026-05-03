using Bogus;
using DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;
using DotCruz.Notifications.Domain.Enums.Notifications;

namespace CommonTestUtilities.Commands.Templates;

public class UpdateTemplateCommandBuilder
{
    public static UpdateTemplateCommand Build(
        Guid? id = null,
        string? code = null,
        string? culture = null,
        string? defaultTitle = null,
        string? body = null,
        NotificationType? type = null)
    {
        var f = new Faker();

        return new UpdateTemplateCommand(
            Id: id ?? Guid.Empty,
            Code: code ?? f.Random.Word(),
            Culture: culture ?? "en-US",
            DefaultTitle: defaultTitle ?? f.Lorem.Sentence(),
            Body: body ?? f.Lorem.Paragraph(),
            Type: type ?? f.PickRandom<NotificationType>()
        );
    }
}

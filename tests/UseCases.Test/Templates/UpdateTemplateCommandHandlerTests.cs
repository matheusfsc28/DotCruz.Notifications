using CommonTestUtilities.Commands.Templates;
using CommonTestUtilities.Entities.Templates;
using CommonTestUtilities.Repositories;
using DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;

namespace UseCases.Test.Templates;

public class UpdateTemplateCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var template = TemplateBuilder.Build();
        var command = UpdateTemplateCommandBuilder.Build(template.Id);
        
        var repository = new TemplateRepositoryBuilder()
            .GetById(template)
            .Build();
            
        var handler = new UpdateTemplateCommandHandler(repository);

        await handler.Handle(command, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task Error_TemplateNotFound()
    {
        var command = UpdateTemplateCommandBuilder.Build();
        var repository = new TemplateRepositoryBuilder().Build();
        var handler = new UpdateTemplateCommandHandler(repository);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.Contains(ResourceMessagesException.TEMPLATE_NOT_FOUND, exception.GetErrorsMessages());
    }
}

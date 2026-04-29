using CommonTestUtilities.Entities.Templates;
using CommonTestUtilities.Repositories;
using DotCruz.Notifications.Application.UseCases.Templates.DeleteTemplate;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;

namespace UseCases.Test.Templates;

public class DeleteTemplateCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var template = TemplateBuilder.Build();
        var command = new DeleteTemplateCommand(template.Id);
        
        var repository = new TemplateRepositoryBuilder()
            .GetById(template)
            .Build();
            
        var handler = new DeleteTemplateCommandHandler(repository);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotNull(template.DeletedAt);
    }

    [Fact]
    public async Task Error_TemplateNotFound()
    {
        var command = new DeleteTemplateCommand(Guid.NewGuid());
        var repository = new TemplateRepositoryBuilder().Build();
        var handler = new DeleteTemplateCommandHandler(repository);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.Contains(ResourceMessagesException.TEMPLATE_NOT_FOUND, exception.GetErrorsMessages());
    }
}

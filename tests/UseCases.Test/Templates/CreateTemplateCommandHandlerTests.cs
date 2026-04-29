using CommonTestUtilities.Commands.Templates;
using CommonTestUtilities.Entities.Templates;
using CommonTestUtilities.Repositories;
using DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;

namespace UseCases.Test.Templates;

public class CreateTemplateCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var command = CreateTemplateCommandBuilder.Build();
        var repository = new TemplateRepositoryBuilder().Build();
        var handler = new CreateTemplateCommandHandler(repository);

        var result = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task Error_TemplateAlreadyExists()
    {
        var command = CreateTemplateCommandBuilder.Build();
        var existingTemplate = TemplateBuilder.Build(code: command.Code, culture: command.Culture);
        
        var repository = new TemplateRepositoryBuilder()
            .GetByCode(existingTemplate)
            .Build();
            
        var handler = new CreateTemplateCommandHandler(repository);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);

        Assert.Contains(ResourceMessagesException.TEMPLATE_ALREADY_EXISTS, exception.GetErrorsMessages());
    }
}

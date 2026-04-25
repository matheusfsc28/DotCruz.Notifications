using CommonTestUtilities.Commands.Notifications;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Factories;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Exceptions.BaseExceptions;

namespace UseCases.Test.Notifications;

public class CreateNotificationCommandHandlerTests
{
    [Theory]
    [ClassData(typeof(NotificationTypeInlineDataTest))]
    public async Task Success(NotificationType type)
    {
        var command = CreateNotificationCommandBuilder.Build(type: type);
        var notification = NotificationBuilder.Build(type);
        
        var strategy = new NotificationFactoryStrategyBuilder(type)
            .Create(notification)
            .Build();
            
        var strategies = new NotificationFactoryStrategyListBuilder()
            .Add(strategy)
            .Build();

        var handler = CreateHandler(strategies);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(notification.Id, result);
    }

    [Fact]
    public async Task Error_NotificationTypeNotSupported()
    {
        var command = CreateNotificationCommandBuilder.Build();
        
        var handler = CreateHandler();

        var act = () => handler.Handle(command, CancellationToken.None);

        await Assert.ThrowsAsync<NotificationTypeNotSupportedException>(act);
    }

    private static CreateNotificationCommandHandler CreateHandler(IEnumerable<INotificationFactoryStrategy>? strategies = null)
    {
        strategies ??= new NotificationFactoryStrategyListBuilder().Build();

        var repository = new NotificationRepositoryBuilder().Build();
        var publishService = new PublishNotificationServiceBuilder().Build();

        return new CreateNotificationCommandHandler(repository, strategies, publishService);
    }
}
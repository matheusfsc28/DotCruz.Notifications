using CommonTestUtilities.Commands.Notifications;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Factories;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;
using DotCruz.Notifications.Contracts.Enums.Notifications;
using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using DotCruz.Notifications.Domain.Interfaces;

namespace UseCases.Test.Notifications;

public class CreateNotificationCommandHandlerTests
{
    [Theory]
    [ClassData(typeof(IntegrationNotificationTypeInlineDataTest))]
    public async Task Success(IntegrationNotificationType type)
    {
        var command = CreateNotificationCommandBuilder.Build(type: type);
        
        var domainType = type switch
        {
            IntegrationNotificationType.Email => NotificationType.Email,
            IntegrationNotificationType.Sms => NotificationType.Sms,
            IntegrationNotificationType.Push => NotificationType.Push,
            _ => throw new ArgumentOutOfRangeException()
        };

        var notification = NotificationBuilder.Build(domainType);
        
        var strategy = new NotificationFactoryStrategyBuilder(domainType)
            .Create(notification)
            .Build();
            
        var strategies = new NotificationFactoryStrategyListBuilder()
            .Add(strategy)
            .Build();

        var handler = CreateHandler(strategies);

        var result = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(notification.Id, result);
    }

    [Fact]
    public async Task Error_NotificationTypeNotSupported()
    {
        var command = CreateNotificationCommandBuilder.Build();
        
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotificationTypeNotSupportedException>(act);

        Assert.Contains(ResourceMessagesException.NOTIFICATION_TYPE_NOT_SUPPORTED, exception.GetErrorsMessages());
    }

    private static CreateNotificationCommandHandler CreateHandler(IEnumerable<INotificationFactoryStrategy>? strategies = null, Template? template = null)
    {
        strategies ??= new NotificationFactoryStrategyListBuilder().Build();

        var repository = new NotificationRepositoryBuilder().Build();
        var templateRepository = new TemplateRepositoryBuilder();
        var publishService = new PublishNotificationServiceBuilder().Build();

        if (template != null)
            templateRepository.GetById(template);

        return new CreateNotificationCommandHandler(repository, templateRepository.Build(), strategies, publishService);
    }
}

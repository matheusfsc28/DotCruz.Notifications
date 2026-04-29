using CommonTestUtilities.Commands.Notifications;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Entities.Templates;
using CommonTestUtilities.InlineData;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using DotCruz.Notifications.Application.UseCases.Notifications.SendNotification;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace UseCases.Test.Notifications;

public class SendNotificationCommandHandlerTests
{
    [Theory]
    [ClassData(typeof(NotificationTypeInlineDataTest))]
    public async Task Success(NotificationType type)
    {
        var template = TemplateBuilder.Build(type: type);
        var notification = NotificationBuilder.Build(type, templateId: template.Id, templateData: new Dictionary<string, object>());
        var command = SendNotificationCommandBuilder.Build(notification.Id);

        var notificationRepository = new NotificationRepositoryBuilder()
            .GetById(notification)
            .Build();

        var templateRepository = new TemplateRepositoryBuilder()
            .GetById(template)
            .Build();

        var templateEngine = new TemplateEngineBuilder()
            .Render("Rendered Body")
            .Build();

        var sender = new NotificationSenderStrategyBuilder(type).Build();
        var senders = new NotificationSenderStrategyListBuilder()
            .Add(sender)
            .Build();

        var handler = CreateHandler(notificationRepository, templateRepository, templateEngine, senders);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(NotificationStatus.Sent, notification.Status);

        if (type == NotificationType.Email)
        {
            Assert.Contains("Rendered Body", notification.Body);
            Assert.Contains("DotCruz Notifications", notification.Body);
        }
        else
        {
            Assert.Equal("Rendered Body", notification.Body);
        }

        Mock.Get(sender).Verify(s => s.SendAsync(notification, It.IsAny<CancellationToken>()), Times.Once);
        Mock.Get(notificationRepository).Verify(r => r.UpdateAsync(notification, It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Theory]
    [ClassData(typeof(NotificationTypeInlineDataTest))]
    public async Task Success_WithoutTemplate(NotificationType type)
    {
        var notification = NotificationBuilder.Build(type);
        var command = SendNotificationCommandBuilder.Build(notification.Id);

        var notificationRepository = new NotificationRepositoryBuilder()
            .GetById(notification)
            .Build();

        var templateRepository = new TemplateRepositoryBuilder().Build();

        var templateEngine = new TemplateEngineBuilder()
            .Render("Rendered Body")
            .Build();

        var sender = new NotificationSenderStrategyBuilder(type).Build();
        var senders = new NotificationSenderStrategyListBuilder()
            .Add(sender)
            .Build();

        var handler = CreateHandler(notificationRepository, templateRepository, templateEngine, senders);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(NotificationStatus.Sent, notification.Status);

        if (type == NotificationType.Email)
        {
            Assert.Contains("Rendered Body", notification.Body);
            Assert.Contains("DotCruz Notifications", notification.Body);
        }
        else
        {
            Assert.Equal("Rendered Body", notification.Body);
        }

        Mock.Get(sender).Verify(s => s.SendAsync(notification, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Success_Email_WrapsContent()
    {
        var type = NotificationType.Email;
        var notification = NotificationBuilder.Build(type, body: "Content");
        var command = SendNotificationCommandBuilder.Build(notification.Id);

        var notificationRepository = new NotificationRepositoryBuilder()
            .GetById(notification)
            .Build();

        var templateEngine = new TemplateEngineBuilder()
            .Render("Rendered Content")
            .Build();

        var sender = new NotificationSenderStrategyBuilder(type).Build();
        var senders = new NotificationSenderStrategyListBuilder()
            .Add(sender)
            .Build();

        var handler = CreateHandler(notificationRepository: notificationRepository, templateEngine: templateEngine, senders: senders);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Contains("DotCruz Notifications", notification.Body);
        Assert.Contains("Rendered Content", notification.Body);
        Assert.Contains("Todos os direitos reservados", notification.Body);
    }

    [Fact]
    public async Task Error_NotificationNotFound()
    {
        var command = SendNotificationCommandBuilder.Build();
        var notificationRepository = new NotificationRepositoryBuilder().Build();

        var handler = CreateHandler(notificationRepository: notificationRepository);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.Contains(ResourceMessagesException.NOTIFICATION_NOT_FOUND, exception.GetErrorsMessages());
    }

    [Fact]
    public async Task Information_NotificationAlreadySent()
    {
        var notification = NotificationBuilder.Build(NotificationType.Email);
        notification.RegisterSuccess(DateTimeOffset.UtcNow);
        var command = SendNotificationCommandBuilder.Build(notification.Id);
        
        var notificationRepository = new NotificationRepositoryBuilder()
            .GetById(notification)
            .Build();
        
        var handler = CreateHandler(notificationRepository: notificationRepository);

        await handler.Handle(command, TestContext.Current.CancellationToken);
        
        Mock.Get(notificationRepository).Verify(r => r.UpdateAsync(notification, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Information_NotificationProcessing()
    {
        var notification = NotificationBuilder.Build(NotificationType.Email);
        notification.MarkAsProcessing();
        var command = SendNotificationCommandBuilder.Build(notification.Id);

        var notificationRepository = new NotificationRepositoryBuilder()
            .GetById(notification)
            .Build();

        var handler = CreateHandler(notificationRepository: notificationRepository);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Mock.Get(notificationRepository).Verify(r => r.UpdateAsync(notification, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Error_NotificationTypeNotSupported()
    {
        var notification = NotificationBuilder.Build(NotificationType.Email);
        var command = SendNotificationCommandBuilder.Build(notification.Id);

        var notificationRepository = new NotificationRepositoryBuilder()
            .GetById(notification)
            .Build();

        var handler = CreateHandler(notificationRepository: notificationRepository);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotificationTypeNotSupportedException>(act);

        Assert.Contains(ResourceMessagesException.NOTIFICATION_TYPE_NOT_SUPPORTED, exception.GetErrorsMessages());
    }

    private static SendNotificationCommandHandler CreateHandler(
        INotificationRepository? notificationRepository = null,
        ITemplateRepository? templateRepository = null,
        ITemplateEngine? templateEngine = null,
        IEnumerable<INotificationSenderStrategy>? senders = null,
        ILogger<SendNotificationCommandHandler>? logger = null)
    {
        return new SendNotificationCommandHandler(
            notificationRepository ?? new NotificationRepositoryBuilder().Build(),
            templateRepository ?? new TemplateRepositoryBuilder().Build(),
            templateEngine ?? new TemplateEngineBuilder().Build(),
            senders ?? new NotificationSenderStrategyListBuilder().Build(),
            logger ?? Mock.Of<ILogger<SendNotificationCommandHandler>>());
    }
}

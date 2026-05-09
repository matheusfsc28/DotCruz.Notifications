using CommonTestUtilities.Entities.Notifications;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using DotCruz.Notifications.Application.UseCases.Notifications.PollScheduledNotifications;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace UseCases.Test.Notifications;

public class PollScheduledNotificationsCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPublishEvents_WhenPendingNotificationsExist()
    {
        var notification = EmailNotificationBuilder.Build(scheduledFor: DateTimeOffset.UtcNow.AddMinutes(-10));
        var notifications = new List<Notification> { notification };

        var repository = new NotificationRepositoryBuilder()
            .GetPendingScheduled(notifications)
            .Build();

        var publishService = new PublishNotificationServiceBuilder().Build();
        var logger = Mock.Of<ILogger<PollScheduledNotificationsCommandHandler>>();

        var handler = new PollScheduledNotificationsCommandHandler(repository, publishService, logger);

        await handler.Handle(new PollScheduledNotificationsCommand(), TestContext.Current.CancellationToken);

        Mock.Get(publishService).Verify(x => x.PublishNotificationCreatedEvent(
            It.Is<Notification>(n => n.Id == notification.Id),
            TestContext.Current.CancellationToken), 
            Times.Once);
    }
}

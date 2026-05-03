using CommonTestUtilities.Entities.Notifications;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Worker.BackgroundServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace Worker.Test.BackgroundServices;

public class ScheduledNotificationPollerTest
{
    [Fact]
    public async Task PollScheduledNotifications_ShouldPublishEvents_WhenPendingNotificationsExist()
    {
        var notification = EmailNotificationBuilder.Build(scheduledFor: DateTimeOffset.UtcNow.AddMinutes(-10));

        var notifications = new List<Notification> { notification };

        var repository = new NotificationRepositoryBuilder()
            .GetPendingScheduled(notifications)
            .Build();

        var publishService = new PublishNotificationServiceBuilder().Build();

        var serviceProvider = new ServiceProviderBuilder()
            .WithService(repository)
            .WithService(publishService)
            .Build();

        var logger = Mock.Of<ILogger<ScheduledNotificationPoller>>();

        var poller = new ScheduledNotificationPollerWrapper(serviceProvider, logger);

        await poller.ExposedPollScheduledNotifications(TestContext.Current.CancellationToken);

        Mock.Get(publishService).Verify(x => x.PublishNotificationCreatedEvent(
            It.Is<Notification>(n => n.Id == notification.Id),
            TestContext.Current.CancellationToken), 
            Times.Once);
    }
}

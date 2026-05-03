using DotCruz.Notifications.Application.Events.Notifications;
using DotCruz.Notifications.Application.UseCases.Notifications.SendNotification;
using DotCruz.Notifications.Worker.Consumers;
using Moq;

namespace Worker.Test.Consumers;

public class NotificationCreatedEventConsumerTest : ConsumerTestBase
{
    [Fact]
    public async Task Consume_ShouldSendNotificationCommand()
    {
        var notificationId = Guid.NewGuid();
        await InitializeHarness(x => x.AddConsumer<NotificationCreatedEventConsumer>());

        await Harness.Bus.Publish(new NotificationCreatedEvent(notificationId), TestContext.Current.CancellationToken);

        Assert.True(await Harness.Consumed.Any<NotificationCreatedEvent>(TestContext.Current.CancellationToken));
        Assert.True(await Harness.GetConsumerHarness<NotificationCreatedEventConsumer>().Consumed.Any<NotificationCreatedEvent>(TestContext.Current.CancellationToken));
        
        var commandFound = false;
        for (int i = 0; i < 10; i++)
        {
            try
            {
                Mock.Get(Mediator).Verify(x => x.Send(
                    It.Is<SendNotificationCommand>(c => c.NotificationId == notificationId),
                    It.IsAny<CancellationToken>()), 
                    Times.Once);
                commandFound = true;
                break;
            }
            catch
            {
                await Task.Delay(200, TestContext.Current.CancellationToken);
            }
        }

        Assert.True(commandFound, "SendNotificationCommand was not sent to Mediator.");
    }
}

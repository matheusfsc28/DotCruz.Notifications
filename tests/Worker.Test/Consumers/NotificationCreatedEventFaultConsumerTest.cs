using DotCruz.Notifications.Application.Events.Notifications;
using DotCruz.Notifications.Application.UseCases.Notifications.RegisterFailureNotification;
using DotCruz.Notifications.Worker.Consumers;
using MassTransit;
using Moq;

namespace Worker.Test.Consumers;

public class NotificationCreatedEventFaultConsumerTest : ConsumerTestBase
{
    [Fact]
    public async Task Consume_ShouldRegisterFailureNotificationCommand()
    {
        var notificationId = Guid.NewGuid();
        await InitializeHarness(x => 
        {
            x.AddConsumer<FailingConsumer>();
            x.AddConsumer<NotificationCreatedEventFaultConsumer>();
        });

        await Harness.Bus.Publish(new NotificationCreatedEvent(notificationId), TestContext.Current.CancellationToken);

        Assert.True(await Harness.Consumed.Any<NotificationCreatedEvent>(TestContext.Current.CancellationToken));
        
        Assert.True(await Harness.Consumed.Any<Fault<NotificationCreatedEvent>>(TestContext.Current.CancellationToken));
        
        var commandFound = false;
        for(int i = 0; i < 10; i++)
        {
            try 
            {
                Mock.Get(Mediator).Verify(x => x.Send(
                    It.Is<RegisterFailureNotificationCommand>(c => 
                        c.NotificationId == notificationId && 
                        c.ErrorsMessage.Any(e => e.Contains("Consumer Failed"))),
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

        Assert.True(commandFound, "RegisterFailureNotificationCommand was not sent to Mediator.");
    }

    private class FailingConsumer : IConsumer<NotificationCreatedEvent>
    {
        public Task Consume(ConsumeContext<NotificationCreatedEvent> context)
        {
            throw new Exception("Consumer Failed");
        }
    }
}

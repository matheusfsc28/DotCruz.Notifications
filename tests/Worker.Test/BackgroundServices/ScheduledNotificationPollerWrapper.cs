using DotCruz.Notifications.Worker.BackgroundServices;
using Microsoft.Extensions.Logging;

namespace Worker.Test.BackgroundServices;

internal class ScheduledNotificationPollerWrapper : ScheduledNotificationPoller
{
    public ScheduledNotificationPollerWrapper(IServiceProvider serviceProvider, ILogger<ScheduledNotificationPoller> logger)
    : base(serviceProvider, logger) { }

    public Task ExposedPollScheduledNotifications(CancellationToken stoppingToken)
    {
        var method = typeof(ScheduledNotificationPoller).GetMethod("PollScheduledNotifications",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (method is null)
            throw new InvalidOperationException("Method PollScheduledNotifications not found.");

        return (Task)method.Invoke(this, [stoppingToken])!;
    }
}

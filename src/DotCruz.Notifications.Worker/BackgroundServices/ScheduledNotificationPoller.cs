using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Worker.BackgroundServices;

public class ScheduledNotificationPoller : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduledNotificationPoller> _logger;
    private readonly TimeSpan _pollingInterval = TimeSpan.FromMinutes(1);

    public ScheduledNotificationPoller(IServiceProvider serviceProvider, ILogger<ScheduledNotificationPoller> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(ResourceLogMessages.POLLER_STARTED);

        using var timer = new PeriodicTimer(_pollingInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await PollScheduledNotifications(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResourceLogMessages.ERROR_POLLING_NOTIFICATIONS);
            }
        }
    }

    private async Task PollScheduledNotifications(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
        var publishService = scope.ServiceProvider.GetRequiredService<IPublishNotificationService>();

        var notifications = await repository.GetPendingScheduledAsync(DateTimeOffset.UtcNow, limit: 100, stoppingToken);

        foreach (var notification in notifications)
        {
            _logger.LogInformation(ResourceLogMessages.PROCESSING_SCHEDULED_NOTIFICATION, notification.Id);

            await publishService.PublishNotificationCreatedEvent(notification, stoppingToken);
        }
    }
}

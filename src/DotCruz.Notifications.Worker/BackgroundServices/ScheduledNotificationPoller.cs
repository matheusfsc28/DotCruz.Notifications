using DotCruz.Notifications.Application.UseCases.Notifications.PollScheduledNotifications;
using DotCruz.Notifications.CrossCutting.Resources;
using MediatR;

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
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new PollScheduledNotificationsCommand(), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResourceLogMessages.ERROR_POLLING_NOTIFICATIONS);
            }
        }
    }
}

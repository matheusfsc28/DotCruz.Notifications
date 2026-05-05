using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Infrastructure.Services.Senders;

public class PushSenderStrategy : INotificationSenderStrategy
{
    private readonly ILogger<PushSenderStrategy> _logger;

    public PushSenderStrategy(ILogger<PushSenderStrategy> logger)
    {
        _logger = logger;
    }

    public NotificationType HandledType => NotificationType.Push;

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Infrastructure.Services.Senders;

public class SmsSenderStrategy : INotificationSenderStrategy
{
    private readonly ILogger<SmsSenderStrategy> _logger;

    public SmsSenderStrategy(ILogger<SmsSenderStrategy> logger)
    {
        _logger = logger;
    }

    public NotificationType HandledType => NotificationType.Sms;

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

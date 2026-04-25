using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Infrastructure.Services.Senders;

public class EmailSenderStrategy : INotificationSenderStrategy
{
    private readonly ILogger<EmailSenderStrategy> _logger;

    public EmailSenderStrategy(ILogger<EmailSenderStrategy> logger)
    {
        _logger = logger;
    }

    public NotificationType HandledType => NotificationType.Email;

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken)
    {
        var email = (EmailNotification)notification;
        
        _logger.LogInformation(ResourceLogMessages.SENDING_EMAIL, email.Recipient, email.Subject);
        
        // Simula latência de rede
        await Task.Delay(500, cancellationToken);
    }
}

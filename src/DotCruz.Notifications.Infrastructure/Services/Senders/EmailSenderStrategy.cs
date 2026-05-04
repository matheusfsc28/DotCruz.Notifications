using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.CrossCutting.Settings;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace DotCruz.Notifications.Infrastructure.Services.Senders;

public class EmailSenderStrategy : INotificationSenderStrategy
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailSenderStrategy> _logger;

    public EmailSenderStrategy(
        IOptions<EmailSettings> emailSettings,
        ILogger<EmailSenderStrategy> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public NotificationType HandledType => NotificationType.Email;

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken)
    {
        var email = (EmailNotification)notification;
        
        _logger.LogInformation(ResourceLogMessages.SENDING_EMAIL, email.Recipient, email.Title);

        using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
        {
            Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
            EnableSsl = _emailSettings.EnableSsl
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
            Subject = email.Title,
            Body = email.Body ?? string.Empty,
            IsBodyHtml = true
        };

        mailMessage.To.Add(email.Recipient);

        await client.SendMailAsync(mailMessage, cancellationToken);
    }
}

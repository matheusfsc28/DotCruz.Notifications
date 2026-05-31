using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.CrossCutting.Settings;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

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

        var message = BuildMessage(email);

        using var client = new SmtpClient();

        await ConfigureClientAsync(client, cancellationToken);

        await client.SendAsync(message!, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    private MimeMessage? BuildMessage(EmailNotification email)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
        message.To.Add(new MailboxAddress(string.Empty, email.Recipient));
        message.Subject = email.Title;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = email.Body ?? string.Empty
        };

        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }

    private async Task ConfigureClientAsync(SmtpClient client, CancellationToken cancellationToken)
    {
        await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.EnableSsl, cancellationToken);
        await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);
    }
}

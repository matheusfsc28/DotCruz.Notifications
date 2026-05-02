using DotCruz.Notifications.Application.Common.Utils;
using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Application.UseCases.Notifications.SendNotification;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ITemplateRepository _templateRepository;
    private readonly ITemplateEngine _templateEngine;
    private readonly IEnumerable<INotificationSenderStrategy> _senders;
    private readonly ILogger<SendNotificationCommandHandler> _logger;

    public SendNotificationCommandHandler(
        INotificationRepository notificationRepository,
        ITemplateRepository templateRepository,
        ITemplateEngine templateEngine,
        IEnumerable<INotificationSenderStrategy> senders,
        ILogger<SendNotificationCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _templateRepository = templateRepository;
        _templateEngine = templateEngine;
        _senders = senders;
        _logger = logger;
    }

    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.NOTIFICATION_NOT_FOUND);

        if (notification.Status == NotificationStatus.Sent || notification.Status == NotificationStatus.Processing)
        {
            _logger.LogInformation(ResourceLogMessages.NOTIFICATION_ALREADY_SENT, request.NotificationId);
            return;
        }

        notification.MarkAsProcessing();
        await _notificationRepository.UpdateAsync(notification, cancellationToken);

        await ProcessTemplateAsync(notification, cancellationToken);

        var sender = _senders.FirstOrDefault(s => s.HandledType == notification.Type) 
            ?? throw new NotificationTypeNotSupportedException();

        await sender.SendAsync(notification, cancellationToken);

        notification.RegisterSuccess(DateTimeOffset.UtcNow);

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
    }

    private async Task ProcessTemplateAsync(Notification notification, CancellationToken cancellationToken)
    {
        var (rawTitle, rawBody) = await GetRawContent(notification, cancellationToken);

        if (!string.IsNullOrWhiteSpace(rawTitle))
        {
            var renderedTitle = _templateEngine.Render(rawTitle, notification.TemplateData);
            notification.SetRenderedTitle(renderedTitle);
        }

        if (!string.IsNullOrWhiteSpace(rawBody))
        {
            var renderedBody = _templateEngine.Render(rawBody, notification.TemplateData);

            if (notification.Type == NotificationType.Email)
                renderedBody = EmailTemplateWrapper.Wrap(renderedBody);

            notification.SetRenderedBody(renderedBody);
        }
    }

    private async Task<(string Title, string Body)> GetRawContent(Notification notification, CancellationToken cancellationToken)
    {
        if (notification.TemplateId.HasValue)
        {
            var template = await _templateRepository.GetByIdAsync(notification.TemplateId.Value, cancellationToken);
            if (template != null)
                return (template.DefaultTitle, template.Body);
        }

        var title = notification switch
        {
            EmailNotification e => e.Title,
            PushNotification p => p.Title,
            _ => string.Empty
        };

        return (title!, notification.Body!);
    }
}

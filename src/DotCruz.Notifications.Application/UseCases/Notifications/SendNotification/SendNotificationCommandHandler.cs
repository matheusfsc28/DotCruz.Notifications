using DotCruz.Notifications.Application.Common.Utils;
using DotCruz.Notifications.CrossCutting.Resources;
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
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken);

        if (notification == null)
            throw new NotFoundException(ResourceMessagesException.NOTIFICATION_NOT_FOUND);

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

    private async Task ProcessTemplateAsync(Domain.Entities.Notifications.Notification notification, CancellationToken cancellationToken)
    {
        var content = notification.Body;

        if (notification.TemplateId.HasValue)
        {
            var template = await _templateRepository.GetByIdAsync(notification.TemplateId.Value, cancellationToken);
            if (template != null)
            {
                content = template.Body;
            }
        }

        if (!string.IsNullOrEmpty(content))
        {
            var renderedBody = _templateEngine.Render(content, notification.TemplateData);

            if (notification.Type == NotificationType.Email)
                renderedBody = EmailTemplateWrapper.Wrap(renderedBody);

            notification.SetRenderedBody(renderedBody);
        }
    }
}

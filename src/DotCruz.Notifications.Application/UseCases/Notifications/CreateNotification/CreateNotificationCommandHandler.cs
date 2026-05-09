using DotCruz.Notifications.Contracts.Enums.Notifications;
using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ITemplateRepository _templateRepository;
    private readonly IEnumerable<INotificationFactoryStrategy> _factories;
    private readonly IPublishNotificationService _publishService;

    public CreateNotificationCommandHandler(
        INotificationRepository notificationRepository,
        ITemplateRepository templateRepository,
        IEnumerable<INotificationFactoryStrategy> factories,
        IPublishNotificationService publishService)
    {
        _notificationRepository = notificationRepository;
        _templateRepository = templateRepository;
        _factories = factories;
        _publishService = publishService;
    }

    public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        Guid? resolvedTemplateId = null;

        if (!string.IsNullOrWhiteSpace(message.TemplateCode))
        {
            var template = await ResolveTemplateAsync(message.TemplateCode, message.Culture, cancellationToken);
            resolvedTemplateId = template?.Id;
        }

        var domainType = MapToDomainType(message.Type);

        var factory = _factories.FirstOrDefault(f => f.Type == domainType)
            ?? throw new NotificationTypeNotSupportedException();

        var notification = factory.Create(
            message.ServiceId,
            message.Recipient,
            message.Culture,
            message.Body,
            message.Title,
            resolvedTemplateId,
            message.TemplateData,
            message.ScheduledFor);

        await _notificationRepository.AddAsync(notification, cancellationToken);
        
        if (notification.ScheduledFor == null || notification.ScheduledFor <= DateTimeOffset.UtcNow)
            await _publishService.PublishNotificationCreatedEvent(notification, cancellationToken);

        return notification.Id;
    }

    private async Task<Template?> ResolveTemplateAsync(string code, string? culture, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByCodeAsync(code, culture ?? "pt-BR", cancellationToken);
        
        if (template == null && culture != "pt-BR")
            template = await _templateRepository.GetByCodeAsync(code, "pt-BR", cancellationToken);

        if (template == null && culture != "en" && culture != "pt-BR")
            template = await _templateRepository.GetByCodeAsync(code, "en", cancellationToken);

        return template;
    }

    private static NotificationType MapToDomainType(IntegrationNotificationType type)
    {
        return type switch
        {
            IntegrationNotificationType.Email => NotificationType.Email,
            IntegrationNotificationType.Sms => NotificationType.Sms,
            IntegrationNotificationType.Push => NotificationType.Push,
            _ => throw new NotificationTypeNotSupportedException()
        };
    }
}

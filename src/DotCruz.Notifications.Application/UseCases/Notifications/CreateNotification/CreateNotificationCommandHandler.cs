using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;
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
        if (request.TemplateId.HasValue)
            await ValidateTemplateExist(request.TemplateId.Value, cancellationToken);

        var factory = _factories.FirstOrDefault(f => f.Type == request.Type)
            ?? throw new NotificationTypeNotSupportedException();

        var notification = factory.Create(
            request.ServiceId,
            request.Recipient,
            request.Culture,
            request.Body,
            request.Title,
            request.TemplateId,
            request.TemplateData,
            request.ScheduledFor);

        await _notificationRepository.AddAsync(notification, cancellationToken);
        
        if (notification.ScheduledFor == null || notification.ScheduledFor <= DateTimeOffset.UtcNow)
            await _publishService.PublishNotificationCreatedEvent(notification, cancellationToken);

        return notification.Id;
    }

    private async Task ValidateTemplateExist(Guid templateId, CancellationToken cancellationToken)
    {
        var _ = await _templateRepository.GetByIdAsync(templateId, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TEMPLATE_NOT_FOUND);
    }
}

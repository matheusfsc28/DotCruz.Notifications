using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
{
    private readonly INotificationRepository _repository;
    private readonly IEnumerable<INotificationFactoryStrategy> _factories;
    private readonly IPublishNotificationService _publishService;

    public CreateNotificationCommandHandler(
        INotificationRepository repository,
        IEnumerable<INotificationFactoryStrategy> factories,
        IPublishNotificationService publishService)
    {
        _repository = repository;
        _factories = factories;
        _publishService = publishService;
    }

    public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var factory = _factories.FirstOrDefault(f => f.Type == request.Type) 
            ?? throw new InvalidOperationException($"No factory found for notification type: {request.Type}");

        var notification = factory.Create(
            request.ServiceId,
            request.Recipient,
            request.Culture,
            request.Body,
            request.Subject,
            request.TemplateId,
            request.TemplateData,
            request.ScheduledFor);

        await _repository.AddAsync(notification, cancellationToken);
        
        await _publishService.PublishNotificationCreatedEvent(notification, cancellationToken);

        return notification.Id;
    }
}

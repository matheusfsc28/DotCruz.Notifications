using DotCruz.Notifications.Application.Events.Notifications;
using DotCruz.Notifications.Application.UseCases.Notifications.SendNotification;
using DotCruz.Notifications.CrossCutting.Resources;
using MassTransit;
using MediatR;

namespace DotCruz.Notifications.Worker.Consumers;

public class NotificationCreatedEventConsumer : IConsumer<NotificationCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<NotificationCreatedEventConsumer> _logger;

    public NotificationCreatedEventConsumer(IMediator mediator, ILogger<NotificationCreatedEventConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<NotificationCreatedEvent> context)
    {
        _logger.LogInformation(ResourceLogMessages.CONSUMING_NOTIFICATION, context.Message.NotificationId);
        
        await _mediator.Send(new SendNotificationCommand(context.Message.NotificationId));
    }
}

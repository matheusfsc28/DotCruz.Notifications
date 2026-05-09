using DotCruz.Notifications.Application.Events.Notifications;
using DotCruz.Notifications.Application.UseCases.Notifications.SendNotification;
using MassTransit;
using MediatR;

namespace DotCruz.Notifications.Worker.Consumers;

public class NotificationCreatedEventConsumer : IConsumer<NotificationCreatedEvent>
{
    private readonly IMediator _mediator;

    public NotificationCreatedEventConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<NotificationCreatedEvent> context)
    {
        await _mediator.Send(new SendNotificationCommand(context.Message.NotificationId), context.CancellationToken);
    }
}

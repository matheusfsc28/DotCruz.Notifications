using DotCruz.Notifications.Application.Events.Notifications;
using DotCruz.Notifications.Application.UseCases.Notifications.RegisterFailureNotification;
using MassTransit;
using MediatR;

namespace DotCruz.Notifications.Worker.Consumers;

public class NotificationCreatedEventFaultConsumer : IConsumer<Fault<NotificationCreatedEvent>>
{
    private readonly IMediator _mediator;

    public NotificationCreatedEventFaultConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<Fault<NotificationCreatedEvent>> context)
    {
        var notificationId = context.Message.Message.NotificationId;
        var exceptions = context.Message.Exceptions.Select(x => x.Message);

        await _mediator.Send(new RegisterFailureNotificationCommand(notificationId, exceptions), context.CancellationToken);
    }
}

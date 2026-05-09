using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;
using DotCruz.Notifications.Contracts.Messages.Notifications.CreateNotification;
using MassTransit;
using MediatR;

namespace DotCruz.Notifications.Worker.Consumers;

public class CreateNotificationConsumer : IConsumer<CreateNotificationMessage>
{
    private readonly IMediator _mediator;

    public CreateNotificationConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<CreateNotificationMessage> context)
    {
        await _mediator.Send(new CreateNotificationCommand(context.Message), context.CancellationToken);
    }
}

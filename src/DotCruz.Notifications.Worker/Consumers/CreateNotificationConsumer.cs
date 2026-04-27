using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;
using DotCruz.Notifications.CrossCutting.Resources;
using MassTransit;
using MediatR;

namespace DotCruz.Notifications.Worker.Consumers;

public class CreateNotificationConsumer : IConsumer<CreateNotificationCommand>
{
    private readonly IMediator _mediator;
    private readonly ILogger<CreateNotificationConsumer> _logger;

    public CreateNotificationConsumer(IMediator mediator, ILogger<CreateNotificationConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateNotificationCommand> context)
    {
        _logger.LogInformation(ResourceLogMessages.CONSUMING_CREATE_NOTIFICATION, context.Message.Recipient);

        await _mediator.Send(context.Message);
    }
}

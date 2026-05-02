using DotCruz.Notifications.Api.Controllers.Base;
using DotCruz.Notifications.Application.DTOs.Base;
using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotCruz.Notifications.Api.Controllers.Notifications;

public class NotificationController : DotCruzNotificationBaseController
{
    public NotificationController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Created("", result);
    }
}

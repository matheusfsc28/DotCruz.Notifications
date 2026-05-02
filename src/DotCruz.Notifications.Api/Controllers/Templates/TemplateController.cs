using DotCruz.Notifications.Api.Controllers.Base;
using DotCruz.Notifications.Application.DTOs.Base;
using DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;
using DotCruz.Notifications.Application.UseCases.Templates.DeleteTemplate;
using DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotCruz.Notifications.Api.Controllers.Templates;

public class TemplateController : DotCruzNotificationBaseController
{
    public TemplateController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Created("", result);
    }

    [HttpPut]
    [Route("{Id}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromRoute] Guid Id, [FromBody] UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        var command = request with { Id = Id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [Route("{Id}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTemplateCommand(Id), cancellationToken);
        return NoContent();
    }
}

using DotCruz.Notifications.Api.Attributes;
using DotCruz.Notifications.Application.DTOs.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotCruz.Notifications.Api.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
[AuthenticatedService]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
public class DotCruzNotificationBaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    public DotCruzNotificationBaseController(IMediator mediator) 
        => _mediator = mediator;
}

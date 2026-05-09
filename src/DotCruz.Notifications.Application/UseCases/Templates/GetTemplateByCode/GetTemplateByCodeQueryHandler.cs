using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.GetTemplateByCode;

public class GetTemplateByCodeQueryHandler : IRequestHandler<GetTemplateByCodeQuery, TemplateResponseDto>
{
    private readonly ITemplateRepository _repository;

    public GetTemplateByCodeQueryHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<TemplateResponseDto> Handle(GetTemplateByCodeQuery request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByCodeAsync(request.Code, request.Culture ?? "pt-BR", cancellationToken);

        if (template == null && request.Culture != "pt-BR")
            template = await _repository.GetByCodeAsync(request.Code, "pt-BR", cancellationToken);

        if (template == null)
            throw new NotFoundException(ResourceMessagesException.TEMPLATE_NOT_FOUND);

        return new TemplateResponseDto(
            template.Id,
            template.Code,
            template.Culture,
            template.DefaultTitle,
            template.Body);
    }
}

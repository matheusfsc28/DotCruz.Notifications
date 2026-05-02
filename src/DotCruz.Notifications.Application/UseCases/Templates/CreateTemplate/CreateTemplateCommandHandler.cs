using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, Guid>
{
    private readonly ITemplateRepository _repository;

    public CreateTemplateCommandHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var existingTemplate = await _repository.GetByCodeAsync(request.Code, request.Culture, cancellationToken);
        if (existingTemplate != null)
            throw new ErrorOnValidationException(ResourceMessagesException.TEMPLATE_ALREADY_EXISTS);

        var template = new Template(
            request.Code,
            request.Culture,
            request.DefaultTitle,
            request.Body,
            request.Type);

        await _repository.AddAsync(template, cancellationToken);

        return template.Id;
    }
}

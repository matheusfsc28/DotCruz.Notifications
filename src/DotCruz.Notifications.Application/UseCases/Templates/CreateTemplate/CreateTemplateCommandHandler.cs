using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.CreateTemplate;

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, Guid>
{
    private readonly ITemplateRepository _repository;
    private readonly ITenantProvider _tenantProvider;

    public CreateTemplateCommandHandler(ITemplateRepository repository, ITenantProvider tenantProvider)
    {
        _repository = repository;
        _tenantProvider = tenantProvider;
    }

    public async Task<Guid> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;
        if (!tenantId.HasValue)
            throw new UnauthorizedException(ResourceMessagesException.TENANT_ID_REQUIRED);

        var existingTemplate = await _repository.GetByCodeAsync(request.Code, request.Culture, cancellationToken);
        if (existingTemplate != null)
            throw new ErrorOnValidationException(ResourceMessagesException.TEMPLATE_ALREADY_EXISTS);

        var template = new Template(
            request.Code,
            request.Culture,
            request.DefaultTitle,
            request.Body,
            request.Type,
            tenantId.Value);

        await _repository.AddAsync(template, cancellationToken);

        return template.Id;
    }
}

using DotCruz.Notifications.Domain.Interfaces.Repositories;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.UpdateTemplate;

public class UpdateTemplateCommandHandler : IRequestHandler<UpdateTemplateCommand>
{
    private readonly ITemplateRepository _repository;

    public UpdateTemplateCommandHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TEMPLATE_NOT_FOUND);

        template.Update(
            request.Code,
            request.Culture,
            request.DefaultSubject,
            request.Body,
            request.Type);
        
        template.Touch();

        await _repository.UpdateAsync(template, cancellationToken);
    }
}

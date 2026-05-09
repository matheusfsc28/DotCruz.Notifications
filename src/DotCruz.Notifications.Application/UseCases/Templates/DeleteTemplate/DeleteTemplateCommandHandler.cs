using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.DeleteTemplate;

public class DeleteTemplateCommandHandler : IRequestHandler<DeleteTemplateCommand>
{
    private readonly ITemplateRepository _repository;

    public DeleteTemplateCommandHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TEMPLATE_NOT_FOUND);

        template.Delete();
        template.Touch();

        await _repository.UpdateAsync(template, cancellationToken);
    }
}

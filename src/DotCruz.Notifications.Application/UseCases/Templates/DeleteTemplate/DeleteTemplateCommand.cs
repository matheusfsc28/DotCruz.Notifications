using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.DeleteTemplate;

public record DeleteTemplateCommand(Guid Id) : IRequest;

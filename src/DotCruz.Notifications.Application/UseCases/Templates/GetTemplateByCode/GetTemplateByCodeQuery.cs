using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Templates.GetTemplateByCode;

public record GetTemplateByCodeQuery(string Code, string? Culture) : IRequest<TemplateResponseDto>;

public record TemplateResponseDto(Guid Id, string Code, string Culture, string Title, string Body);

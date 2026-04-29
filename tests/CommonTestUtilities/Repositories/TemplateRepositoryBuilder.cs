using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class TemplateRepositoryBuilder
{
    private readonly Mock<ITemplateRepository> _repository;

    public TemplateRepositoryBuilder()
    {
        _repository = new Mock<ITemplateRepository>();
    }

    public TemplateRepositoryBuilder GetById(Template? template)
    {
        if (template is not null)
        {
            _repository.Setup(r => r.GetByIdAsync(template.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(template);
        }

        return this;
    }

    public TemplateRepositoryBuilder GetByCode(Template? template)
    {
        if (template is not null)
        {
            _repository.Setup(r => r.GetByCodeAsync(template.Code, template.Culture, It.IsAny<CancellationToken>()))
                .ReturnsAsync(template);
        }

        return this;
    }

    public ITemplateRepository Build() => _repository.Object;
}

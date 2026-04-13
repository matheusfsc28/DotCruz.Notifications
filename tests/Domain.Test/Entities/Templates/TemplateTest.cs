using CommonTestUtilities.Entities.Templates;
using DotCruz.Notification.Exceptions;
using DotCruz.Notification.Exceptions.BaseExceptions;
using FluentAssertions;
using Xunit;

namespace Domain.Test.Entities.Templates;

public class TemplateTest
{
    [Fact]
    public void Success()
    {
        var template = TemplateBuilder.Build();

        template.Should().NotBeNull();
        template.Code.Should().NotBeNullOrWhiteSpace();
        template.Culture.Should().NotBeNullOrWhiteSpace();
        template.DefaultSubject.Should().NotBeNullOrWhiteSpace();
        template.Body.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Error_Code_Empty()
    {
        var action = () => TemplateBuilder.Build(code: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.CODE_EMPTY));
    }

    [Fact]
    public void Error_DefaultSubject_Empty()
    {
        var action = () => TemplateBuilder.Build(defaultSubject: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.DEFAULT_SUBJECT_EMPTY));
    }

    [Fact]
    public void Error_Body_Empty()
    {
        var action = () => TemplateBuilder.Build(body: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.BODY_EMPTY));
    }
}

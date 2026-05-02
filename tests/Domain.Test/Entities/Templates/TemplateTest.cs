using CommonTestUtilities.Entities.Templates;
using DotCruz.Notifications.Exceptions;
using DotCruz.Notifications.Exceptions.BaseExceptions;
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
        template.DefaultTitle.Should().NotBeNullOrWhiteSpace();
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
    public void Error_DefaultTitle_Empty()
    {
        var action = () => TemplateBuilder.Build(defaultTitle: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.DEFAULT_TITLE_EMPTY));
    }

    [Fact]
    public void Error_Body_Empty()
    {
        var action = () => TemplateBuilder.Build(body: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.BODY_EMPTY));
    }
}

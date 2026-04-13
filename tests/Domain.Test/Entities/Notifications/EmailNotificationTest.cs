using CommonTestUtilities.Entities.Notifications;
using DotCruz.Notification.Exceptions;
using DotCruz.Notification.Exceptions.BaseExceptions;
using FluentAssertions;
using Xunit;

namespace Domain.Test.Entities.Notifications;

public class EmailNotificationTest
{
    [Fact]
    public void Success()
    {
        var notification = EmailNotificationBuilder.Build();

        notification.Should().NotBeNull();
        notification.ServiceId.Should().NotBeEmpty();
        notification.Recipient.Should().NotBeNullOrWhiteSpace();
        notification.Subject.Should().NotBeNullOrWhiteSpace();
        notification.Culture.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Error_ServiceId_Empty()
    {
        var action = () => EmailNotificationBuilder.Build(serviceId: Guid.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.SERVICE_ID_EMPTY));
    }

    [Fact]
    public void Error_Recipient_Empty()
    {
        var action = () => EmailNotificationBuilder.Build(recipient: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.RECIPIENT_EMPTY));
    }

    [Fact]
    public void Error_Subject_Empty()
    {
        var action = () => EmailNotificationBuilder.Build(subject: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.SUBJECT_EMPTY));
    }

    [Fact]
    public void Error_Body_And_Template_Empty()
    {
        var action = () => EmailNotificationBuilder.Build(body: string.Empty, templateId: null);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.BODY_OR_TEMPLATE_REQUIRED));
    }
}

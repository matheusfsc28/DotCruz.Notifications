using CommonTestUtilities.Entities.Notifications;
using DotCruz.Notification.Exceptions;
using DotCruz.Notification.Exceptions.BaseExceptions;
using FluentAssertions;
using Xunit;

namespace Domain.Test.Entities.Notifications;

public class PushNotificationTest
{
    [Fact]
    public void Success()
    {
        var notification = PushNotificationBuilder.Build();

        notification.Should().NotBeNull();
        notification.ServiceId.Should().NotBeEmpty();
        notification.Recipient.Should().NotBeNullOrWhiteSpace();
        notification.Title.Should().NotBeNullOrWhiteSpace();
        notification.Culture.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Error_Title_Empty()
    {
        var action = () => PushNotificationBuilder.Build(title: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.SUBJECT_EMPTY));
    }
}

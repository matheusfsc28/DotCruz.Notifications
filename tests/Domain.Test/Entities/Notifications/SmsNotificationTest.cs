using CommonTestUtilities.Entities.Notifications;
using DotCruz.Notification.Exceptions;
using DotCruz.Notification.Exceptions.BaseExceptions;
using FluentAssertions;
using Xunit;

namespace Domain.Test.Entities.Notifications;

public class SmsNotificationTest
{
    [Fact]
    public void Success()
    {
        var notification = SmsNotificationBuilder.Build();

        notification.Should().NotBeNull();
        notification.ServiceId.Should().NotBeEmpty();
        notification.Recipient.Should().NotBeNullOrWhiteSpace();
        notification.Culture.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Error_PhoneNumber_Empty()
    {
        var action = () => SmsNotificationBuilder.Build(phoneNumber: string.Empty);

        action.Should().ThrowExactly<ErrorOnValidationException>()
            .Where(e => e.GetErrorsMessages().Contains(ResourceMessagesException.RECIPIENT_EMPTY));
    }
}

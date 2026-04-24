using CommonTestUtilities.Commands.Notifications;
using DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;

namespace Validators.Test.Notifications;

public class CreateNotificationCommandValidatorTests
{
    [Fact]
    public async Task Success()
    {
        var validator = new CreateNotificationCommandValidator();
        var request = CreateNotificationCommandBuilder.Build();

        var result = await validator.ValidateAsync(request);

        Assert.True(result.IsValid);
    }
}

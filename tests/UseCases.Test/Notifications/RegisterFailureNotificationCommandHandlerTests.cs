using CommonTestUtilities.Commands.Notifications;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using DotCruz.Notifications.Application.UseCases.Notifications.RegisterFailureNotification;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using Moq;

namespace UseCases.Test.Notifications;

public class RegisterFailureNotificationCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var notification = NotificationBuilder.Build(NotificationType.Email);
        var command = RegisterFailureNotificationCommandBuilder.Build(notificationId: notification.Id);

        var repository = new NotificationRepositoryBuilder()
            .GetById(notification)
            .Build();

        var handler = new RegisterFailureNotificationCommandHandler(repository);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(NotificationStatus.Failed, notification.Status);
        Assert.NotEmpty(notification.Errors);
        
        var expectedErrorMessage = string.Join(" | ", command.ErrorsMessage);
        Assert.Contains(expectedErrorMessage, notification.Errors.First().Message);
        
        Mock.Get(repository).Verify(r => r.UpdateAsync(notification, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Error_NotificationNotFound()
    {
        var command = RegisterFailureNotificationCommandBuilder.Build();
        var repository = new NotificationRepositoryBuilder().Build();

        var handler = new RegisterFailureNotificationCommandHandler(repository);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.Contains(ResourceMessagesException.NOTIFICATION_NOT_FOUND, exception.GetErrorsMessages());
    }
}

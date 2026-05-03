using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class NotificationRepositoryBuilder
{
    private readonly Mock<INotificationRepository> _repository;

    public NotificationRepositoryBuilder()
    {
        _repository = new Mock<INotificationRepository>();
    }

    public NotificationRepositoryBuilder GetById(Notification? notification)
    {
        if (notification is not null)
        {
            _repository.Setup(r => r.GetByIdAsync(notification.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(notification);
        }

        return this;
    }

    public NotificationRepositoryBuilder GetPendingScheduled(IEnumerable<Notification> notifications)
    {
        _repository.Setup(r => r.GetPendingScheduledAsync(It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        return this;
    }

    public INotificationRepository Build() => _repository.Object;
}

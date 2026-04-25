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

    public INotificationRepository Build() => _repository.Object;
}
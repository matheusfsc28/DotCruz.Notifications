using DotCruz.Notifications.Domain.Entities.Notifications;

namespace DotCruz.Notifications.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

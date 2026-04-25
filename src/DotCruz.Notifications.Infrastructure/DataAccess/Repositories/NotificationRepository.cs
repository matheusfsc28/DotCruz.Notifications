using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace DotCruz.Notifications.Infrastructure.DataAccess.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        await _context.Notifications.InsertOneAsync(notification, cancellationToken: cancellationToken);
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Find(n => n.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetPendingScheduledAsync(DateTimeOffset referenceDate, int limit, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Find(n => n.Status == NotificationStatus.Pending && 
                       n.ScheduledFor != null && 
                       n.ScheduledFor <= referenceDate)
            .Limit(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken)
    {
        await _context.Notifications.ReplaceOneAsync(
            n => n.Id == notification.Id,
            notification,
            cancellationToken: cancellationToken);
    }
}

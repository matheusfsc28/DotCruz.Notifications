using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace DotCruz.Notifications.Infrastructure.DataAccess.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;
    private readonly ITenantProvider _tenantProvider;

    public NotificationRepository(NotificationDbContext context, ITenantProvider tenantProvider)
    {
        _context = context;
        _tenantProvider = tenantProvider;
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        await _context.Notifications.InsertOneAsync(notification, cancellationToken: cancellationToken);
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;
        return await _context.Notifications
            .Find(n => n.Id == id && (tenantId == null || n.TenantId == tenantId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetPendingScheduledAsync(DateTimeOffset referenceDate, int limit, CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;
        return await _context.Notifications
            .Find(n => n.Status == NotificationStatus.Pending && 
                       n.ScheduledFor != null && 
                       n.ScheduledFor <= referenceDate &&
                       (tenantId == null || n.TenantId == tenantId))
            .Limit(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;
        await _context.Notifications.ReplaceOneAsync(
            n => n.Id == notification.Id && (tenantId == null || n.TenantId == tenantId),
            notification,
            cancellationToken: cancellationToken);
    }
}

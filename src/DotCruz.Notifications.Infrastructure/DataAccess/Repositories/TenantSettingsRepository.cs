using DotCruz.Notifications.Domain.Entities.Tenants;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotCruz.Notifications.Infrastructure.DataAccess.Repositories
{
    public class TenantSettingsRepository : ITenantSettingsRepository
    {
        private readonly NotificationDbContext _context;

        public TenantSettingsRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TenantSettings settings, CancellationToken cancellationToken)
        {
            await _context.TenantSettings.InsertOneAsync(settings, cancellationToken: cancellationToken);
        }

        public async Task<TenantSettings?> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            return await _context.TenantSettings
                .Find(t => t.TenantId == tenantId && t.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(TenantSettings settings, CancellationToken cancellationToken)
        {
            await _context.TenantSettings.ReplaceOneAsync(
                t => t.Id == settings.Id && t.TenantId == settings.TenantId,
                settings,
                cancellationToken: cancellationToken);
        }
    }
}

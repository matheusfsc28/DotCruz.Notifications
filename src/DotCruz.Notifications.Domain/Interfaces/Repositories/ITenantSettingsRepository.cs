using DotCruz.Notifications.Domain.Entities.Tenants;

namespace DotCruz.Notifications.Domain.Interfaces.Repositories
{
    public interface ITenantSettingsRepository
    {
        Task AddAsync(TenantSettings settings, CancellationToken cancellationToken);
        Task UpdateAsync(TenantSettings settings, CancellationToken cancellationToken);
        Task<TenantSettings?> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
    }
}

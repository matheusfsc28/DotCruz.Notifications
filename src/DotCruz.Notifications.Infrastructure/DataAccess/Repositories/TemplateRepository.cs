using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace DotCruz.Notifications.Infrastructure.DataAccess.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly NotificationDbContext _context;
    private readonly ITenantProvider _tenantProvider;

    public TemplateRepository(NotificationDbContext context, ITenantProvider tenantProvider)
    {
        _context = context;
        _tenantProvider = tenantProvider;
    }

    public async Task AddAsync(Template template, CancellationToken cancellationToken)
    {
        await _context.Templates.InsertOneAsync(template, cancellationToken: cancellationToken);
    }

    public async Task<Template?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;
        return await _context.Templates
            .Find(t => t.Id == id && t.DeletedAt == null && (tenantId == null || t.TenantId == tenantId || t.TenantId == Guid.Empty))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Template?> GetByCodeAsync(string code, string culture, CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;
        return await _context.Templates
            .Find(t => t.Code == code && t.Culture == culture && t.DeletedAt == null && t.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Template?> GetGlobalByCodeAsync(string code, string culture, CancellationToken cancellationToken)
    {
        return await _context.Templates
            .Find(t => t.Code == code && t.Culture == culture && t.DeletedAt == null && t.TenantId == Guid.Empty)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(Template template, CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;
        await _context.Templates.ReplaceOneAsync(
            t => t.Id == template.Id && (tenantId == null || t.TenantId == tenantId),
            template,
            cancellationToken: cancellationToken);
    }
}

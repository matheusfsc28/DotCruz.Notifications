using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace DotCruz.Notifications.Infrastructure.DataAccess.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly NotificationDbContext _context;

    public TemplateRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Template template, CancellationToken cancellationToken)
    {
        await _context.Templates.InsertOneAsync(template, cancellationToken: cancellationToken);
    }

    public async Task<Template?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Templates
            .Find(t => t.Id == id && t.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Template?> GetByCodeAsync(string code, string culture, CancellationToken cancellationToken)
    {
        return await _context.Templates
            .Find(t => t.Code == code && t.Culture == culture && t.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(Template template, CancellationToken cancellationToken)
    {
        await _context.Templates.ReplaceOneAsync(
            t => t.Id == template.Id,
            template,
            cancellationToken: cancellationToken);
    }
}

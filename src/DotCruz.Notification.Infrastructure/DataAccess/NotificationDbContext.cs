using DotCruz.Notification.Domain.Entities.Templates;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NotificationEntity = DotCruz.Notification.Domain.Entities.Notifications.Notification;

namespace DotCruz.Notification.Infrastructure.DataAccess;

public class NotificationDbContext
{
    private readonly IMongoDatabase _database;

    public NotificationDbContext(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<NotificationEntity> Notifications => _database.GetCollection<NotificationEntity>("notifications");
    public IMongoCollection<Template> Templates => _database.GetCollection<Template>("templates");

    public async Task CreateIndexesAsync()
    {
        var notificationIndexes = Notifications.Indexes;
        
        var statusIndex = new CreateIndexModel<NotificationEntity>(
            Builders<NotificationEntity>.IndexKeys.Ascending(x => x.Status).Descending(x => x.CreatedAt),
            new CreateIndexOptions { Background = true }
        );

        await notificationIndexes.CreateOneAsync(statusIndex);
    }
}

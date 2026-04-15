using DotCruz.Notification.Infrastructure.DataAccess;
using DotCruz.Notification.Infrastructure.DataAccess.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DotCruz.Notification.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddMongoDb(services, configuration);
    }

    private static void AddMongoDb(IServiceCollection services, IConfiguration configuration)
    {
        MongoDbMappings.Configure();

        var mongoDbSection = configuration.GetSection("MongoDbSettings");
        var settings = mongoDbSection.Get<MongoDbSettings>() 
                       ?? throw new InvalidOperationException("MongoDbSettings section is missing in configuration.");

        services.Configure<MongoDbSettings>(mongoDbSection);

        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoClientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
            
            mongoClientSettings.RetryWrites = true;
            mongoClientSettings.RetryReads = true;
            mongoClientSettings.MaxConnectionPoolSize = 100;
            mongoClientSettings.ConnectTimeout = TimeSpan.FromSeconds(10);

            return new MongoClient(mongoClientSettings);
        });

        services.AddSingleton<NotificationDbContext>();
    }
}

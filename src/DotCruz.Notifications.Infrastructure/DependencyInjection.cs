using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Infrastructure.Messaging;
using DotCruz.Notifications.Infrastructure.DataAccess;
using DotCruz.Notifications.Infrastructure.DataAccess.Mappings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DotCruz.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddMongoDb(services, configuration);
        AddMassTransit(services, configuration);
        
        services.AddScoped<IPublishNotificationService, PublishNotificationService>();
    }

    private static void AddMassTransit(IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
            ?? throw new InvalidOperationException("RabbitMqSettings section is missing in configuration.");

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.Host, (ushort)rabbitMqSettings.Port, "/", h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
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

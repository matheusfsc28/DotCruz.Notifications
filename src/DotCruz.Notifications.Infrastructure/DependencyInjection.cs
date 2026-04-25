using DotCruz.Notifications.CrossCutting.Settings;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using DotCruz.Notifications.Infrastructure.DataAccess;
using DotCruz.Notifications.Infrastructure.DataAccess.Mappings;
using DotCruz.Notifications.Infrastructure.DataAccess.Repositories;
using DotCruz.Notifications.Infrastructure.Services.Messaging;
using DotCruz.Notifications.Infrastructure.Services.Senders;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotCruz.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddMassTransit(services, configuration);
        AddExternalServices(services, configuration);
        AddMongoDb(services, configuration);
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
    }

    private static void AddExternalServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPublishNotificationService, PublishNotificationService>();
        
        services.AddScoped<INotificationSenderStrategy, EmailSenderStrategy>();
        services.AddScoped<INotificationSenderStrategy, SmsSenderStrategy>();
        services.AddScoped<INotificationSenderStrategy, PushSenderStrategy>();
    }

    private static void AddMassTransit(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

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

        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoClientSettings = MongoClientSettings.FromConnectionString(configuration.GetConnectionString("MongoDb"));

            mongoClientSettings.RetryWrites = true;
            mongoClientSettings.RetryReads = true;
            mongoClientSettings.MaxConnectionPoolSize = 100;
            mongoClientSettings.ConnectTimeout = TimeSpan.FromSeconds(10);

            return new MongoClient(mongoClientSettings);
        });

        services.AddSingleton<NotificationDbContext>();
    }
}

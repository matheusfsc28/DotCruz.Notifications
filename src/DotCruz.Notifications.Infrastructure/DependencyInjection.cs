using Amazon.Scheduler;
using Amazon.SQS;
using DotCruz.Notifications.Application.Common.Interfaces;
using DotCruz.Notifications.CrossCutting.Settings;
using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using DotCruz.Notifications.Infrastructure.DataAccess;
using DotCruz.Notifications.Infrastructure.DataAccess.Mappings;
using DotCruz.Notifications.Infrastructure.DataAccess.Repositories;
using DotCruz.Notifications.Infrastructure.Services.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Reflection;

namespace DotCruz.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, Assembly? consumerAssembly = null)
    {
        ConfigureAwsSdk(services, configuration);
        AddExternalServices(services);
        AddMongoDb(services, configuration);
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
    }

    private static void AddExternalServices(IServiceCollection services)
    {
        services.AddScoped<IPublishNotificationService, PublishNotificationService>();
        services.AddScoped<INotificationScheduler, EventBridgeNotificationScheduler>();
    }

    private static void ConfigureAwsSdk(IServiceCollection services, IConfiguration configuration)
    {
        var awsSettings = configuration.GetSection("Settings:AWS").Get<AwsSettings>();

        services.AddSingleton<IAmazonSQS>(sp =>
        {
            var config = new AmazonSQSConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsSettings!.Region),
            };

            if (!string.IsNullOrEmpty(awsSettings.AccessKey) && !string.IsNullOrEmpty(awsSettings.SecretKey))
            {
                return new AmazonSQSClient(awsSettings.AccessKey, awsSettings.SecretKey, config);
            }

            return new AmazonSQSClient(config);
        });

        services.AddSingleton<IAmazonScheduler>(sp =>
        {
            var config = new AmazonSchedulerConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsSettings!.Region)
            };

            if (!string.IsNullOrEmpty(awsSettings.AccessKey) && !string.IsNullOrEmpty(awsSettings.SecretKey))
            {
                return new AmazonSchedulerClient(awsSettings.AccessKey, awsSettings.SecretKey, config);
            }

            return new AmazonSchedulerClient(config);
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

using DotCruz.Notifications.CrossCutting.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotCruz.Notifications.CrossCutting;

public static class DependencyInjection
{
    public static void AddCrossCutting(this IServiceCollection services, IConfiguration configuration)
    {
        AddRabbitMqSettings(services, configuration);
        AddMongoDbSettings(services, configuration);
    }

    private static void AddRabbitMqSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(
            configuration.GetSection("Settings:RabbitMqSettings")
        );
    }

    private static void AddMongoDbSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection("Settings:MongoDbSettings")
        );
    }
}

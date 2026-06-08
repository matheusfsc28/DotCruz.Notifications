using DotCruz.Notifications.CrossCutting.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotCruz.Notifications.CrossCutting;

public static class DependencyInjection
{
    public static void AddCrossCutting(this IServiceCollection services, IConfiguration configuration)
    {
        AddMongoDbSettings(services, configuration);
        AddAwsSettings(services, configuration);
    }

    private static void AddMongoDbSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection("Settings:MongoDbSettings")
        );
    }

    public static void AddAwsSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsSettings>(
            configuration.GetSection("Settings:AWS")
        );
    }
}

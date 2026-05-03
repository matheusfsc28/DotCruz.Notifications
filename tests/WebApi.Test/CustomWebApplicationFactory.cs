using CommonTestUtilities.Entities;
using CommonTestUtilities.Entities.Templates;
using DotCruz.Notifications.CrossCutting.Settings;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Entities.Templates;
using DotCruz.Notifications.Domain.Enums.Notifications;
using DotCruz.Notifications.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private Template _template = default!;
    private Notification _notification = default!;
    private string _apiToken = default!;
    private readonly string _databaseName = $"Notifications_Test_{Guid.NewGuid():N}";
    private string? _databaseConnectionString = null;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                _databaseConnectionString = configuration.GetConnectionString("MongoDb");

                if (string.IsNullOrEmpty(_databaseConnectionString))
                    throw new InvalidOperationException("Could not find connection string for tests");

                RemoveService<IMongoClient>(services);
                RemoveService<IOptions<MongoDbSettings>>(services);
                RemoveService<NotificationDbContext>(services);

                var mongoClient = new MongoClient(_databaseConnectionString);
                services.AddSingleton<IMongoClient>(mongoClient);

                services.Configure<MongoDbSettings>(options =>
                {
                    options.DatabaseName = _databaseName;
                });

                services.AddSingleton<NotificationDbContext>();

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

                StartDatabase(dbContext);
                SetApiToken(configuration);
            });
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor is not null)
            services.Remove(descriptor);
    }

    public Guid GetTemplateId() => _template.Id;
    public string GetApiToken() => _apiToken;

    private void StartDatabase(NotificationDbContext dbContext)
    {
        _template = TemplateBuilder.Build();
        _notification = NotificationBuilder.Build(NotificationType.Email);

        dbContext.Templates.InsertOne(_template);
        dbContext.Notifications.InsertOne(_notification);
    }

    private void SetApiToken(IConfiguration configuration)
        =>  _apiToken = configuration.GetValue<string>("Settings:ApiKey")!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            var mongoClient = new MongoClient(_databaseConnectionString);
            mongoClient.DropDatabase(_databaseName);
        }

        base.Dispose(disposing);
    }
}

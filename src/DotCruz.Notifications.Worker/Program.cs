using DotCruz.Notifications.Application;
using DotCruz.Notifications.CrossCutting;
using DotCruz.Notifications.Infrastructure;
using DotCruz.Notifications.Worker.BackgroundServices;
using DotCruz.Notifications.Worker.Consumers;
using MassTransit;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddCrossCutting(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<NotificationCreatedEventConsumer>();
    busConfigurator.AddConsumer<CreateNotificationConsumer>();

    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHostedService<ScheduledNotificationPoller>();

var host = builder.Build();
host.Run();

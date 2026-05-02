using DotCruz.Notifications.Application;
using DotCruz.Notifications.CrossCutting;
using DotCruz.Notifications.Infrastructure;
using DotCruz.Notifications.Worker.BackgroundServices;
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
builder.Services.AddInfrastructure(builder.Configuration, typeof(Program).Assembly);
builder.Services.AddApplication();

builder.Services.AddHostedService<ScheduledNotificationPoller>();

var host = builder.Build();
host.Run();

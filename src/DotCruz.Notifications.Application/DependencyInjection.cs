using DotCruz.Notifications.Application.Common.Behaviors;
using DotCruz.Notifications.Application.Common.Services;
using DotCruz.Notifications.Application.Factories.Notifications;
using DotCruz.Notifications.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DotCruz.Notifications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddMediatR(services);
        AddValidators(services);
        AddServices(services);
        AddNotificationFactoryStrategies(services);

        return services;
    }

    public static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(NotificationFailureBehavior<,>));
            }
        );
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ITemplateEngine, FluidTemplateEngine>();
    }

    private static void AddNotificationFactoryStrategies(IServiceCollection services)
    {
        services.AddScoped<INotificationFactoryStrategy, EmailFactoryStrategy>();
        services.AddScoped<INotificationFactoryStrategy, SmsFactoryStrategy>();
        services.AddScoped<INotificationFactoryStrategy, PushFactoryStrategy>();
    }
}

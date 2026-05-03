using CommonTestUtilities.Services;
using MassTransit;
using MassTransit.Testing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Worker.Test.Consumers;

public abstract class ConsumerTestBase : IAsyncDisposable
{
    private ServiceProvider? _serviceProvider;
    protected ITestHarness Harness { get; private set; } = default!;
    protected IMediator Mediator { get; private set; } = default!;

    protected async Task InitializeHarness(Action<IBusRegistrationConfigurator> configureConsumers)
    {
        Mediator = new MediatorBuilder().Build();

        _serviceProvider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                configureConsumers(x);
            })
            .AddSingleton(Mediator)
            .AddLogging()
            .BuildServiceProvider(true);

        Harness = _serviceProvider.GetRequiredService<ITestHarness>();
        await Harness.Start();
    }

    public async ValueTask DisposeAsync()
    {
        if (_serviceProvider != null)
        {
            await _serviceProvider.DisposeAsync();
        }
    }
}

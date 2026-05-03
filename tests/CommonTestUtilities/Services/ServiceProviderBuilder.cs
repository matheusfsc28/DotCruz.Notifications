using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CommonTestUtilities.Services;

public class ServiceProviderBuilder
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock;
    private readonly Mock<IServiceScope> _scopeMock;

    public ServiceProviderBuilder()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _scopeFactoryMock = new Mock<IServiceScopeFactory>();
        _scopeMock = new Mock<IServiceScope>();

        _serviceProviderMock
            .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(_scopeFactoryMock.Object);

        _scopeFactoryMock
            .Setup(x => x.CreateScope())
            .Returns(_scopeMock.Object);

        _scopeMock
            .Setup(x => x.ServiceProvider)
            .Returns(_serviceProviderMock.Object);
    }

    public ServiceProviderBuilder WithService<T>(T service)
    {
        _serviceProviderMock
            .Setup(x => x.GetService(typeof(T)))
            .Returns(service!);

        return this;
    }

    public IServiceProvider Build() => _serviceProviderMock.Object;
}

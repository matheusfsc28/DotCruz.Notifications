using MediatR;
using Moq;

namespace CommonTestUtilities.Services;

public class MediatorBuilder
{
    private readonly Mock<IMediator> _mock;

    public MediatorBuilder()
    {
        _mock = new Mock<IMediator>();
    }

    public IMediator Build() => _mock.Object;
}

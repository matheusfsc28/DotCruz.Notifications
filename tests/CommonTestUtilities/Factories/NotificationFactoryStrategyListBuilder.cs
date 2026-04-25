using DotCruz.Notifications.Domain.Interfaces;

namespace CommonTestUtilities.Factories;

public class NotificationFactoryStrategyListBuilder
{
    private readonly List<INotificationFactoryStrategy> _strategies = [];

    public NotificationFactoryStrategyListBuilder Add(INotificationFactoryStrategy strategy)
    {
        _strategies.Add(strategy);
        return this;
    }

    public IEnumerable<INotificationFactoryStrategy> Build() => _strategies;
}
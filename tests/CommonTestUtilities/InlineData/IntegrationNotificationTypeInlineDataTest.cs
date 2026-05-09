using DotCruz.Notifications.Contracts.Enums.Notifications;
using System.Collections;

namespace CommonTestUtilities.InlineData;

public class IntegrationNotificationTypeInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [IntegrationNotificationType.Email];
        yield return [IntegrationNotificationType.Sms];
        yield return [IntegrationNotificationType.Push];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using DotCruz.Notifications.Domain.Enums.Notifications;
using System.Collections;

namespace CommonTestUtilities.InlineData;

public class NotificationTypeInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [NotificationType.Email];
        yield return [NotificationType.Sms];
        yield return [NotificationType.Push];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

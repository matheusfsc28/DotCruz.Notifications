using System;

namespace DotCruz.Notifications.Domain.Interfaces
{
    public interface ITenantProvider
    {
        Guid? TenantId { get; }
    }
}

namespace DotCruz.Notifications.Domain.Entities.Base
{
    public abstract class TenantEntity : BaseEntity
    {
        public Guid TenantId { get; protected set; }
        public void SetTenantId(Guid? tenantId) => TenantId = tenantId ?? TenantId;
    }
}

namespace DotCruz.Notifications.Domain.Interfaces
{
    public interface ISmtpConfigService
    {
        Task SaveAsync(Guid tenantId, string host, int port, string username, string password, string fromName, CancellationToken cancellationToken);
    }
}

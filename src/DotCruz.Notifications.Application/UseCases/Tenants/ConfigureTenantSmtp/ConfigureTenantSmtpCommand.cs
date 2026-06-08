using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Tenants.ConfigureTenantSmtp
{
    public record ConfigureTenantSmtpCommand(
        string Host,
        int Port,
        string Username,
        string Password,
        string FromName
    ) : IRequest;
}

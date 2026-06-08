using DotCruz.Notifications.Domain.Interfaces;
using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotCruz.Notifications.Application.UseCases.Tenants.ConfigureTenantSmtp
{
    public class ConfigureTenantSmtpCommandHandler : IRequestHandler<ConfigureTenantSmtpCommand>
    {
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly ITenantProvider _tenantProvider;

        public ConfigureTenantSmtpCommandHandler(ISmtpConfigService smtpConfigService, ITenantProvider tenantProvider)
        {
            _smtpConfigService = smtpConfigService;
            _tenantProvider = tenantProvider;
        }

        public async Task Handle(ConfigureTenantSmtpCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.TenantId;
            if (!tenantId.HasValue)
                throw new UnauthorizedException(ResourceMessagesException.TENANT_ID_REQUIRED);

            await _smtpConfigService.SaveAsync(
                tenantId.Value,
                request.Host,
                request.Port,
                request.Username,
                request.Password,
                request.FromName,
                cancellationToken
            );
        }
    }
}

using DotCruz.Notifications.Domain.Entities.Base;
using System;

namespace DotCruz.Notifications.Domain.Entities.Tenants
{
    public class TenantSettings : TenantEntity
    {
        public string HeaderHtml { get; private set; } = string.Empty;
        public string FooterHtml { get; private set; } = string.Empty;

        private TenantSettings() { }

        public TenantSettings(Guid tenantId, string headerHtml, string footerHtml)
        {
            SetTenantId(tenantId);
            HeaderHtml = headerHtml;
            FooterHtml = footerHtml;
        }

        public void UpdateBranding(string headerHtml, string footerHtml)
        {
            HeaderHtml = headerHtml;
            FooterHtml = footerHtml;
        }
    }
}

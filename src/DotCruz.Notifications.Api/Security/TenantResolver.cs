using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using DotCruz.Notifications.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotCruz.Notifications.Api.Security
{
    public class TenantResolver : ITenantProvider
    {
        private const string AUTHENTICATION_TYPE = "Bearer";
        private const string SUPER_ADMIN_ROLE = "SuperAdmin";
        private const string TENANT_ID_CLAIM = "tenant_id";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public TenantResolver(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public Guid? TenantId => ResolveTenantId();

        private Guid? ResolveTenantId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return null;

            if (context.Request.Headers.TryGetValue("X-Api-Key", out var apiKeyHeader))
            {
                if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var apiKeyTenantHeader) && Guid.TryParse(apiKeyTenantHeader, out var apiKeyTenantId))
                {
                    return apiKeyTenantId;
                }
                return null;
            }

            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith($"{AUTHENTICATION_TYPE} ", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);

            var token = authHeader[$"{AUTHENTICATION_TYPE} ".Length..].Trim();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (!jwtSecurityTokenHandler.CanReadToken(token))
                throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);

            var signingKey = _configuration["Settings:Jwt:SigningKey"];
            if (string.IsNullOrEmpty(signingKey))
                throw new UnauthorizedException(ResourceMessagesException.TOKEN_INVALID);

            var key = Encoding.UTF8.GetBytes(signingKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var user = principal;

                if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                    throw new UnauthorizedException(ResourceMessagesException.TOKEN_INVALID);

                var isSuperAdmin = user.IsInRole(SUPER_ADMIN_ROLE) || user.HasClaim(ClaimTypes.Role, SUPER_ADMIN_ROLE);

                if (isSuperAdmin && context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantHeader) && Guid.TryParse(tenantHeader, out var impersonatedId))
                    return impersonatedId;

                var tenantClaim = user.FindFirst(TENANT_ID_CLAIM)?.Value;
                if (!Guid.TryParse(tenantClaim, out var tenantId))
                    return null;
                
                return tenantId;
            }
            catch (Exception)
            {
                throw new UnauthorizedException(ResourceMessagesException.TOKEN_INVALID);
            }
        }
    }
}

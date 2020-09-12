using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xera.MultiTenant.AspNetCore.Contracts;
using Xera.MultiTenant.AspNetCore.Manager;
using Xera.MultiTenant.AspNetCore.Options;
using Xera.MultiTenant.AspNetCore.Providers;

namespace Xera.MultiTenant.AspNetCore.Middlewares
{
    internal class MultiTenantCurrentTenantMiddleware
    {
        private readonly RequestDelegate _next;

        public MultiTenantCurrentTenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext httpContext, 
            ITenantProvider tenantProvider,
            ITenantManager tenantManager,
            IOptions<MultiTenantOptions> multiTenantOptionsWrapper,
            ILogger<MultiTenantCurrentTenantMiddleware> logger = null)
        {
            var multiTenantOptionsOptions = multiTenantOptionsWrapper.Value;

            if (multiTenantOptionsOptions.TenantIdentificationSource != TenantIdentificationSource.None)
            {
                Guid tenantId;
                var tenantIdentificationSourceName = multiTenantOptionsOptions.TenantIdentificationSourceName;

                if (multiTenantOptionsOptions.TenantIdentificationSource == TenantIdentificationSource.Headers)
                {
                    if (httpContext.Request.Headers.ContainsKey(tenantIdentificationSourceName))
                    {
                        var tenantIdAsString = httpContext.Request.Headers[tenantIdentificationSourceName];

                        if (!Guid.TryParse(tenantIdAsString, out tenantId))
                        {
                            logger?.LogWarning(
                                $"Cannot set TenantId. Check header {tenantIdentificationSourceName} has valid value");
                        }
                    }
                }
                else if (multiTenantOptionsOptions.TenantIdentificationSource == TenantIdentificationSource.Claims)
                {
                    var claim = httpContext.User.Claims.SingleOrDefault(c => c.Type == tenantIdentificationSourceName);

                    var tenantIdAsString = string.Empty;

                    if (claim != null)
                    {
                        tenantIdAsString = claim.Value;
                    }

                    if (!Guid.TryParse(tenantIdAsString, out tenantId))
                    {
                        logger?.LogWarning(
                            $"Cannot set TenantId. Check header {tenantIdentificationSourceName} has valid value");
                    }
                }
                else if (multiTenantOptionsOptions.TenantIdentificationSource == TenantIdentificationSource.Custom)
                {
                    tenantId = multiTenantOptionsOptions.TenantIdentificationCustomProvider(httpContext);
                }

                var tenant = tenantManager.GetById(tenantId);
                tenantProvider.SetCurrentTenant(tenant);
            }

            await _next(httpContext);
        }
    }
}
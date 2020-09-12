using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xera.MultiTenant.AspNetCore.Contracts;
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
            IOptions<MultiTenantOptions> multiTenantOptionsWrapper,
            ILogger<MultiTenantCurrentTenantMiddleware> logger = null)
        {
            var multiTenantOptionsOptions = multiTenantOptionsWrapper.Value;

            if (multiTenantOptionsOptions.TenantIdentificationSource != TenantIdentificationSource.None)
            {
                Guid tenantId;

                if (multiTenantOptionsOptions.TenantIdentificationSource == TenantIdentificationSource.Headers)
                {
                    if (httpContext.Request.Headers.ContainsKey(
                        multiTenantOptionsOptions.TenantIdentificationSourceName))
                    {
                        var tenantIdAsString =
                            httpContext.Request.Headers[multiTenantOptionsOptions.TenantIdentificationSourceName];

                        if (!Guid.TryParse(tenantIdAsString, out tenantId))
                        {
                            logger?.LogWarning(
                                $"Cannot set TenantId. Check header {multiTenantOptionsOptions.TenantIdentificationSourceName} has valid value");
                        }
                    }
                }
                else if (multiTenantOptionsOptions.TenantIdentificationSource == TenantIdentificationSource.Custom)
                {
                    tenantId = multiTenantOptionsOptions.TenantIdentificationCustomProvider(httpContext);
                }

                var tenant = new Tenant(tenantId);

                tenantProvider.SetCurrentTenant(tenant);
            }

            await _next(httpContext);
        }
    }
}
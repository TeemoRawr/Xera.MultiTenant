using System;
using Microsoft.AspNetCore.Http;

namespace Xera.MultiTenant.AspNetCore.Options
{
    public class MultiTenantOptions
    {
        public MultiTenantOptions()
        {
            TenantIdentificationSource = TenantIdentificationSource.Headers;
            TenantIdentificationSourceName = "X-TENANT";
            TenantIdentificationCustomProvider = httpContext => Guid.Empty;
        }

        public TenantIdentificationSource TenantIdentificationSource { get; internal set; }
        public string TenantIdentificationSourceName { get; internal set; }
        public Func<HttpContext, Guid> TenantIdentificationCustomProvider { get; internal set; }
    }
}
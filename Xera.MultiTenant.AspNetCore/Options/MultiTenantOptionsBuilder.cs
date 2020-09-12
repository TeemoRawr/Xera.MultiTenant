using System;
using Microsoft.AspNetCore.Http;

namespace Xera.MultiTenant.AspNetCore.Options
{
    public class MultiTenantOptionsBuilder
    {
        internal MultiTenantOptions Options { get; }

        public MultiTenantOptionsBuilder()
        {
            Options = new MultiTenantOptions();
        }

        /// <summary>
        /// Set way how Tenant will be set.
        /// </summary>
        /// <param name="tenantIdentificationSource"></param>
        /// <returns></returns>
        public MultiTenantOptionsBuilder SetTenantIdentificationSource(TenantIdentificationSource tenantIdentificationSource)
        {
            Options.TenantIdentificationSource = tenantIdentificationSource;
            return this;
        }
        
        public MultiTenantOptionsBuilder SetTenantIdentificationSourceName(string tenantIdentificationSourceName)
        {
            Options.TenantIdentificationSourceName = tenantIdentificationSourceName;
            return this;
        }

        public MultiTenantOptionsBuilder SetTenantIdentificationCustomProvider(Func<HttpContext, Guid> func)
        {
            Options.TenantIdentificationCustomProvider = func;
            return this;
        }
    }
}
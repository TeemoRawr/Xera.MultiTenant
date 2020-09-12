using System;
using Microsoft.AspNetCore.Http;
using Xera.MultiTenant.AspNetCore.Options;
using Xera.MultiTenant.AspNetCore.Storages;

namespace Xera.MultiTenant.AspNetCore.Extensions
{
    public class MultiTenantConfigurationBuilder
    {
        public MultiTenantConfigurationBuilder()
        {
            Options = new MultiTenantOptions();
            TenantStorage = new MemoryTenantStorage();
        }

        public MultiTenantOptions Options { get; private set; }
        public ITenantStorage TenantStorage { get; private set; }

        /// <summary>
        /// Set way how Tenant will be set.
        /// </summary>
        /// <param name="tenantIdentificationSource"></param>
        /// <returns></returns>
        public MultiTenantConfigurationBuilder SetTenantIdentificationSource(TenantIdentificationSource tenantIdentificationSource)
        {
            Options.TenantIdentificationSource = tenantIdentificationSource;
            return this;
        }
        
        public MultiTenantConfigurationBuilder SetTenantIdentificationSourceName(string tenantIdentificationSourceName)
        {
            Options.TenantIdentificationSourceName = tenantIdentificationSourceName;
            return this;
        }

        public MultiTenantConfigurationBuilder SetTenantIdentificationCustomProvider(Func<HttpContext, Guid> func)
        {
            Options.TenantIdentificationCustomProvider = func;
            return this;
        }

        /// <summary>
        /// Set source of tenants. Default is use <see cref="MemoryTenantStorage"/>
        /// </summary>
        /// <param name="tenantStorage"></param>
        /// <returns></returns>
        public MultiTenantConfigurationBuilder SetTenantStorage(ITenantStorage tenantStorage)
        {
            TenantStorage = tenantStorage;
            return this;
        }
    }
}
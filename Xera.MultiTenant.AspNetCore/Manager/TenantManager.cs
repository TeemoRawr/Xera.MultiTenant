using System;
using Xera.MultiTenant.AspNetCore.Contracts;
using Xera.MultiTenant.AspNetCore.Storages;

namespace Xera.MultiTenant.AspNetCore.Manager
{
    public class TenantManager : ITenantManager
    {
        private readonly ITenantStorage _tenantStorage;

        public TenantManager(ITenantStorage tenantStorage)
        {
            _tenantStorage = tenantStorage;
        }

        public void AddTenant(Tenant tenant)
        {
            _tenantStorage.AddTenant(tenant);
        }

        public void RemoveMemoryTenant(Tenant tenant)
        {
            _tenantStorage.RemoveMemoryTenant(tenant);
        }

        public Tenant GetById(Guid tenantId)
        {
            return _tenantStorage.GetById(tenantId);
        }
    }
}

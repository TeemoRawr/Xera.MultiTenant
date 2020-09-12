using System;

namespace Xera.MultiTenant.AspNetCore.Contracts
{
    public class Tenant
    {
        internal Tenant(Guid tenantId)
        {
            TenantId = tenantId;
        }

        public Guid TenantId { get; }
    }
}
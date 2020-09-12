using System;
using Xera.MultiTenant.AspNetCore.Contracts;

namespace Xera.MultiTenant.AspNetCore.Manager
{
    public interface ITenantManager
    {
        void AddTenant(Tenant tenant);
        void RemoveMemoryTenant(Tenant tenant);
        Tenant GetById(Guid tenantId);
    }
}
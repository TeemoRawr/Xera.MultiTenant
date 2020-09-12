using System;
using Xera.MultiTenant.AspNetCore.Contracts;

namespace Xera.MultiTenant.AspNetCore.Storages
{
    public interface ITenantStorage
    {
        void AddTenant(Tenant tenant);
        void RemoveMemoryTenant(Tenant tenant);
        Tenant GetById(Guid tenantId);
    }
}
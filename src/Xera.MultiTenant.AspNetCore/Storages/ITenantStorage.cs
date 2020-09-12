using System;
using System.Collections.Generic;
using Xera.MultiTenant.AspNetCore.Contracts;

namespace Xera.MultiTenant.AspNetCore.Storages
{
    public interface ITenantStorage
    {
        void AddTenant(Tenant tenant);
        void RemoveTenant(Tenant tenant);
        Tenant GetById(Guid tenantId);
        IEnumerable<Tenant> GetAll();
    }
}
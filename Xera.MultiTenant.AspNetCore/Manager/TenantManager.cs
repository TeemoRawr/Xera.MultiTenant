using System;
using System.Collections.Generic;
using System.Linq;
using Xera.MultiTenant.AspNetCore.Contracts;

namespace Xera.MultiTenant.AspNetCore.Manager
{
    public class TenantManager : ITenantManager
    {
        private readonly ICollection<Tenant> _tenants;

        public TenantManager()
        {
            _tenants = new List<Tenant>();
        }

        public void AddMemoryTenant(Tenant tenant)
        {
            if (_tenants.Any(t => t.TenantId == tenant.TenantId))
            {
                throw new Exception("Cannot add tenant with the same id");
            }

            _tenants.Add(tenant);
        }

        public void RemoveMemoryTenant(Tenant tenant)
        {
            if (_tenants.All(t => t.TenantId != tenant.TenantId))
            {
                throw new Exception($"Cannot find tenant with id {tenant.TenantId}");
            }

            _tenants.Remove(tenant);
        }
    }
}

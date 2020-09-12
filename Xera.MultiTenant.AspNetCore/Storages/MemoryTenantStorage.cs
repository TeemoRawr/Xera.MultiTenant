﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xera.MultiTenant.AspNetCore.Contracts;
using Xera.MultiTenant.AspNetCore.Manager;

namespace Xera.MultiTenant.AspNetCore.Storages
{
    public class MemoryTenantStorage : ITenantStorage
    {
        private readonly ICollection<Tenant> _tenants;

        public MemoryTenantStorage()
        {
            _tenants = new List<Tenant>();
        }

        public MemoryTenantStorage(IEnumerable<Tenant> tenants)
        {
            _tenants = new List<Tenant>();
        }

        public void AddTenant(Tenant tenant)
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

        public Tenant GetById(Guid tenantId)
        {
            return _tenants.SingleOrDefault(tenant => tenant.TenantId == tenantId);
        }
    }
}
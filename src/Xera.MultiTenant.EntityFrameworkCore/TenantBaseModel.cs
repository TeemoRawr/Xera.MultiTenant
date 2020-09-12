using System;

namespace Xera.MultiTenant.EntityFrameworkCore
{
    public abstract class TenantBaseModel
    {
        protected TenantBaseModel()
        {
        }

        protected TenantBaseModel(Guid tenantId)
        {
            TenantId = tenantId;
        }

        public Guid TenantId { get; private set; }
    }
}
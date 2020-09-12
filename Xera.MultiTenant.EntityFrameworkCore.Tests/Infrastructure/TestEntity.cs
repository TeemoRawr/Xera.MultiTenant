using System;

namespace Xera.MultiTenant.EntityFrameworkCore.Tests.Infrastructure
{
    public class TestEntity : TenantBaseModel
    {
        private TestEntity()
        {
        }

        public TestEntity(Guid tenantId) : base(tenantId)
        {
        }

        public int Id { get; private set; }
    }
}
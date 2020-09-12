using System;
using Microsoft.EntityFrameworkCore;

namespace Xera.MultiTenant.EntityFrameworkCore.Tests.Infrastructure
{
    public class TestDbContext : MultiTenantDbContext<TestDbContext>
    {
        public TestDbContext(Guid tenantId) : base(tenantId)
        {
        }

        public DbSet<TestEntity> TestEntities { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase("test");
        }
    }
}
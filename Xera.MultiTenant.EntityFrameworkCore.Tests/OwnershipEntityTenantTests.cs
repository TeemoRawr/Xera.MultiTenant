using System;
using System.Linq;
using FluentAssertions;
using Xera.MultiTenant.EntityFrameworkCore.Tests.Infrastructure;
using Xunit;

namespace Xera.MultiTenant.EntityFrameworkCore.Tests
{
    public class OwnershipEntityTenantTests
    {
        [Fact]
        public void ShouldShowOnlyOwnedTenantEntities()
        {
            var tenantIdOne = Guid.NewGuid();
            var tenantIdTwo = Guid.NewGuid();

            using var tenantOneContext = new TestDbContext(tenantIdOne);
            using var tenantTwoContext = new TestDbContext(tenantIdTwo);

            var firstTestEntity = new TestEntity(tenantIdOne);
            var secondTestEntity = new TestEntity(tenantIdTwo);

            tenantOneContext.TestEntities.Add(firstTestEntity);
            tenantTwoContext.TestEntities.Add(secondTestEntity);

            tenantOneContext.SaveChanges();
            tenantTwoContext.SaveChanges();

            var testEntityFromTenantOneContext = tenantOneContext.TestEntities.ToList();

            testEntityFromTenantOneContext.Count.Should().Be(1);
            testEntityFromTenantOneContext.First().TenantId.Should().Be(tenantIdOne);

            var testEntityFromTenantTwoContext = tenantTwoContext.TestEntities.ToList();

            testEntityFromTenantTwoContext.Count.Should().Be(1);
            testEntityFromTenantTwoContext.First().TenantId.Should().Be(tenantIdTwo);
        }

        [Fact]
        public void ShouldNotAllowSyncSaveOtherTenantEntity()
        {
            var expectedException = Assert.Throws<InvalidOperationException>(() =>
            {
                var tenantIdOne = Guid.NewGuid();
                var tenantIdTwo = Guid.NewGuid();

                using var tenantOneContext = new TestDbContext(tenantIdOne);

                var firstTestEntity = new TestEntity(tenantIdOne);
                var secondTestEntity = new TestEntity(tenantIdTwo);

                tenantOneContext.TestEntities.Add(firstTestEntity);
                tenantOneContext.TestEntities.Add(secondTestEntity);

                tenantOneContext.SaveChanges();
            });

            expectedException.Message.Should().Be("Cannot add entity for other tenant");
        }
    }
}

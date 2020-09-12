using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xera.MultiTenant.EntityFrameworkCore.Tests.Infrastructure;
using Xunit;

namespace Xera.MultiTenant.EntityFrameworkCore.Tests
{
    public class AutoAssignTenantTests
    {
        [Fact]
        public async Task ShouldNotAllowAsyncSaveOtherTenantEntity()
        {
            var expectedException = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var tenantIdOne = Guid.NewGuid();
                var tenantIdTwo = Guid.NewGuid();

                await using var tenantOneContext = new TestDbContext(tenantIdOne);

                var firstTestEntity = new TestEntity(tenantIdOne);
                var secondTestEntity = new TestEntity(tenantIdTwo);

                await tenantOneContext.TestEntities.AddAsync(firstTestEntity);
                await tenantOneContext.TestEntities.AddAsync(secondTestEntity);

                await tenantOneContext.SaveChangesAsync();
            });

            expectedException.Message.Should().Be("Cannot add entity for other tenant");
        }

        [Fact]
        public void ShouldAutoAssignTenantIdOnSyncSave()
        {
            var tenantIdOne = Guid.NewGuid();

            using var tenantOneContext = new TestDbContext(tenantIdOne);

            var firstTestEntity = new TestEntity(default);

            tenantOneContext.TestEntities.Add(firstTestEntity);

            tenantOneContext.SaveChanges();

            var testEntityFromTenantOneContext = tenantOneContext.TestEntities.First();
            testEntityFromTenantOneContext.TenantId.Should().Be(tenantIdOne);
        }

        [Fact]
        public async Task ShouldAutoAssignTenantIdOnAsyncSave()
        {
            var tenantIdOne = Guid.NewGuid();

            await using var tenantOneContext = new TestDbContext(tenantIdOne);

            var firstTestEntity = new TestEntity(default);

            await tenantOneContext.TestEntities.AddAsync(firstTestEntity);

            await tenantOneContext.SaveChangesAsync();

            var testEntityFromTenantOneContext = tenantOneContext.TestEntities.First();
            testEntityFromTenantOneContext.TenantId.Should().Be(tenantIdOne);

        }
    }
}
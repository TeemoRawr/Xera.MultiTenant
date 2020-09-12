using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using SemanticComparison.Fluent;
using Xera.MultiTenant.AspNetCore.Contracts;
using Xera.MultiTenant.AspNetCore.Storages;
using Xunit;

namespace Xera.MultiTenant.AspNetCore.Test.Infrastructure
{
    public class MemoryTenantStorageTests
    {
        private readonly IFixture _fixture;

        public MemoryTenantStorageTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void DefaultInitializationShouldNotContainsAnyTenants()
        {
            // arrange
            var memoryTenantStorage = new MemoryTenantStorage();

            // act
            var tenants = memoryTenantStorage.GetAll();

            // assert
            tenants.Should().BeEmpty();
        }

        [Fact]
        public void DefaultInitializationShouldContainsAllAssignedTenants()
        {
            // arrange
            var tenants = _fixture.CreateMany<Tenant>().ToList();
            var tenantsLikeness = tenants.AsSource().OfLikeness<IEnumerable<Tenant>>();
            var sut = new MemoryTenantStorage(tenants);

            // act
            var tenantsFromStorage = sut.GetAll();

            // assert
            tenantsLikeness.Should().Be(tenantsFromStorage);
        }

        [Fact]
        public void TenantShouldBeAdded()
        {
            // arrange
            var tenant = _fixture.Create<Tenant>();
            var sut = new MemoryTenantStorage();

            // act
            sut.AddTenant(tenant);

            // assert
            var tenants = sut.GetAll().ToList();
            tenants.Should().HaveCount(1);
            tenants.FirstOrDefault().Should().Be(tenant);
        }

        [Fact]
        public void TenantShouldBeRemoved()
        {
            // arrange
            var tenant = _fixture.Create<Tenant>();
            var sut = new MemoryTenantStorage(new List<Tenant>
            {
                tenant
            });

            // act
            sut.RemoveTenant(tenant);

            // assert
            var tenants = sut.GetAll().ToList();
            tenants.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldFindTenantById()
        {
            // arrange
            var tenants = _fixture.CreateMany<Tenant>().ToList();
            var firstTenant = tenants.First();
            var sut = new MemoryTenantStorage(tenants);

            // act
            var tenant = sut.GetById(firstTenant.TenantId);

            // assert
            tenant.Should().Be(firstTenant);
        }
    }
}
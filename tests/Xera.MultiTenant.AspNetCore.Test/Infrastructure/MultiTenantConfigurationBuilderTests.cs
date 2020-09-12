using System;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xera.MultiTenant.AspNetCore.Extensions;
using Xera.MultiTenant.AspNetCore.Options;
using Xera.MultiTenant.AspNetCore.Storages;
using Xunit;

namespace Xera.MultiTenant.AspNetCore.Test.Infrastructure
{
    public class MultiTenantConfigurationBuilderTests
    {
        private readonly IFixture _fixture;

        public MultiTenantConfigurationBuilderTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ShouldSetTenantIdentificationSource()
        {
            // arrange
            var source = _fixture.Create<TenantIdentificationSource>();
            var sut = new MultiTenantConfigurationBuilder();

            // act
            sut.SetTenantIdentificationSource(source);

            // assert
            sut.Options.TenantIdentificationSource.Should().Be(source);
        }

        [Fact]
        public void ShouldSetTenantIdentificationSourceName()
        {
            // arrange
            var identificationSourceName = _fixture.Create<string>();
            var sut = new MultiTenantConfigurationBuilder();

            // act
            sut.SetTenantIdentificationSourceName(identificationSourceName);

            // assert
            sut.Options.TenantIdentificationSourceName.Should().Be(identificationSourceName);
        }

        [Fact]
        public void ShouldSetTenantIdentificationCustomProvider()
        {
            // arrange
            var func = _fixture.Create<Func<HttpContext, Guid>>();
            var sut = new MultiTenantConfigurationBuilder();

            // act
            sut.SetTenantIdentificationCustomProvider(func);

            // assert
            sut.Options.TenantIdentificationCustomProvider.Should().Be(func);
        }

        [Fact]
        public void ShouldSetTenantStorage()
        {
            // arrange
            var tenantStorageMock = new Mock<ITenantStorage>();
            var tenantStorage = tenantStorageMock.Object;
            var sut = new MultiTenantConfigurationBuilder();

            // act
            sut.SetTenantStorage(tenantStorage);

            // assert
            sut.TenantStorage.Should().Be(tenantStorage);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Xera.MultiTenant.EntityFrameworkCore
{
    public abstract class MultiTenantDbContext<TContext> : DbContext where TContext : DbContext
    {
        protected MultiTenantDbContext(Guid tenantId)
        {
            TenantId = tenantId;
        }

        protected MultiTenantDbContext(Guid tenantId, DbContextOptions options) : base(options)
        {
            TenantId = tenantId;
        }

        protected MultiTenantDbContext(Guid tenantId, DbContextOptions<TContext> options) : base(options)
        {
            TenantId = tenantId;
        }

        public Guid TenantId { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var tenantBaseModelTypes = GetType().GetProperties()
                .Select(p => p.PropertyType)
                .Where(p => p.IsGenericType)
                .Where(p => p.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(p => p.GetGenericArguments().First())
                .Where(t => t.BaseType == typeof(TenantBaseModel))
                .ToList();

            foreach (var tenantBaseModelType in tenantBaseModelTypes)
            {
                var parameter = Expression.Parameter(tenantBaseModelType);
                var entityTenantIdProperty = Expression.Property(parameter, nameof(TenantBaseModel.TenantId));

                var tenantIdPropertyInfo = GetType().GetProperty("TenantId") ?? throw new Exception($"Invalid configuration of DbContext");
                var dbContextTenantIdProperty = Expression.Property(Expression.Constant(this), tenantIdPropertyInfo);

                var filter = Expression.Lambda(Expression.Equal(entityTenantIdProperty, dbContextTenantIdProperty), parameter);

                modelBuilder.Entity(tenantBaseModelType).HasQueryFilter(filter);
            }
        }
        
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AutoAssignTenantId();
            CheckTenantAssignment();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            AutoAssignTenantId();
            CheckTenantAssignment();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void CheckTenantAssignment()
        {
            var tenantBaseModels = GetTrackedTenantBaseModels();

            if(tenantBaseModels.Any(t => t.TenantId != TenantId))
            {
                throw new InvalidOperationException("Cannot add entity for other tenant");
            }
        }

        private void AutoAssignTenantId()
        {
            var tenantBaseModelsWithoutSetTenantId = GetTrackedTenantBaseModels()
                .Where(t => t.TenantId == default)
                .ToList();

            foreach (var tenantBaseModel in tenantBaseModelsWithoutSetTenantId)
            {
                typeof(TenantBaseModel)
                    .GetProperty(nameof(TenantBaseModel.TenantId))
                    .SetValue(tenantBaseModel, TenantId);
            }
        }

        private IEnumerable<TenantBaseModel> GetTrackedTenantBaseModels()
        {
            return ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .Cast<TenantBaseModel>()
                .ToList();
        }
    }
}

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xera.MultiTenant.AspNetCore.Manager;
using Xera.MultiTenant.AspNetCore.Middlewares;
using Xera.MultiTenant.AspNetCore.Options;
using Xera.MultiTenant.AspNetCore.Providers;
using Xera.MultiTenant.AspNetCore.Storages;

namespace Xera.MultiTenant.AspNetCore.Extensions
{
    public static class MultiTenantExtensions
    {
        /// <summary>
        /// Add Required services to use MultiTenant flow application
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="multiTenantOptionsBuilderAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddMultiTenant(
            this IServiceCollection serviceCollection, 
            Action<MultiTenantConfigurationBuilder> multiTenantOptionsBuilderAction = null)
        {
            var multiTenantOptionsBuilder = new MultiTenantConfigurationBuilder();

            multiTenantOptionsBuilderAction?.Invoke(multiTenantOptionsBuilder);

            var multiTenantOptionsWrapper = new OptionsWrapper<MultiTenantOptions>(multiTenantOptionsBuilder.Options);

            serviceCollection.AddSingleton<IOptions<MultiTenantOptions>>(provider => multiTenantOptionsWrapper);
            serviceCollection.AddSingleton<ITenantStorage>(provider => multiTenantOptionsBuilder.TenantStorage);

            serviceCollection.AddSingleton<ITenantManager, TenantManager>();
            serviceCollection.AddScoped<ITenantProvider, TenantProvider>();

            return serviceCollection;
        }

        /// <summary>
        /// Configure application HTTP request pipeline to use MultiTenant flow
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<MultiTenantCurrentTenantMiddleware>();

            return applicationBuilder;
        }
    }
}
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xera.MultiTenant.AspNetCore.Manager;
using Xera.MultiTenant.AspNetCore.Middlewares;
using Xera.MultiTenant.AspNetCore.Options;
using Xera.MultiTenant.AspNetCore.Providers;

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
            Action<MultiTenantOptionsBuilder> multiTenantOptionsBuilderAction = null)
        {
            var multiTenantOptionsBuilder = new MultiTenantOptionsBuilder();

            multiTenantOptionsBuilderAction?.Invoke(multiTenantOptionsBuilder);

            var optionsWrapper = new OptionsWrapper<MultiTenantOptions>(multiTenantOptionsBuilder.Options);

            serviceCollection.AddSingleton<IOptions<MultiTenantOptions>>(provider => optionsWrapper);
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
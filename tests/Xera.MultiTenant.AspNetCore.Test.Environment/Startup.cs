using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xera.MultiTenant.AspNetCore.Contracts;
using Xera.MultiTenant.AspNetCore.Extensions;
using Xera.MultiTenant.AspNetCore.Options;
using Xera.MultiTenant.AspNetCore.Storages;

namespace Xera.MultiTenant.AspNetCore.Test.Environment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMultiTenant(builder =>
            {
                builder.SetTenantIdentificationSource(TenantIdentificationSource.Custom);
                builder.SetTenantIdentificationCustomProvider(context => Guid.NewGuid());

                builder.SetTenantStorage(new MemoryTenantStorage(new List<Tenant>
                {
                    Tenant.Create(Guid.NewGuid(), "", "", "", 0, "", "")
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMultiTenant();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

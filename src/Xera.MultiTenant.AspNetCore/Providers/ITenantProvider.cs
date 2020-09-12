using Xera.MultiTenant.AspNetCore.Contracts;

namespace Xera.MultiTenant.AspNetCore.Providers
{
    public interface ITenantProvider
    {
        Tenant GetCurrentTenant();
        void SetCurrentTenant(Tenant tenant);
    }
}
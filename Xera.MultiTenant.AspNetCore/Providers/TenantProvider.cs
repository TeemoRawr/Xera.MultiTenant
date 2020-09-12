using Xera.MultiTenant.AspNetCore.Contracts;

namespace Xera.MultiTenant.AspNetCore.Providers
{
    internal class TenantProvider : ITenantProvider
    {
        private Tenant _currentTenant;

        public Tenant GetCurrentTenant()
        {
            return _currentTenant;
        }

        public void SetCurrentTenant(Tenant tenant)
        {
            _currentTenant = tenant;
        }
    }
}
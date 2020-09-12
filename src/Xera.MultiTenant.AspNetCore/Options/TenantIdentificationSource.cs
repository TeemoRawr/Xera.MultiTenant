namespace Xera.MultiTenant.AspNetCore.Options
{
    public enum TenantIdentificationSource
    {
        Headers,
        Claims,
        None,
        Custom
    }
}
using Microsoft.AspNetCore.Mvc;
using Xera.MultiTenant.AspNetCore.Contracts;
using Xera.MultiTenant.AspNetCore.Providers;

namespace Xera.MultiTenant.AspNetCore.Test.Environment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITenantProvider _tenantProvider;

        public WeatherForecastController(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        [HttpGet]
        public Tenant Get()
        {
            return _tenantProvider.GetCurrentTenant();
        }
    }
}

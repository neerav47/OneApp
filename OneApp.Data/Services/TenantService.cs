
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace OneApp.Data.Services;

public class TenantService(
    IHttpContextAccessor _httpContextAccessor,
    ILogger<TenantService> _logger) : ITenantService
{
    private Guid _tenantId;

    public Guid? GetTenantId()
    {
        var tenant = _httpContextAccessor.HttpContext?.User.FindFirstValue("tenant");

        if (tenant != null && Guid.TryParse(tenant, out var tenantId))
        {
            this._tenantId = tenantId;
            _logger.LogInformation($"{nameof(GetTenantId)} extraction successfull.");
        }

        return this._tenantId;
    }
}

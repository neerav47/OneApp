
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace OneApp.Data.Services;

public class TenantService(
    IHttpContextAccessor _httpContextAccessor,
    ILogger<TenantService> _logger) : ITenantService
{
    private Guid _tenantId;
    private Guid _userId;

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
    public Guid? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

        if (user != null && Guid.TryParse(user, out var userId))
        {
            this._userId = userId;
            _logger.LogInformation($"{nameof(GetUserId)} extraction successfull.");
        }

        return this._userId;
    }
}

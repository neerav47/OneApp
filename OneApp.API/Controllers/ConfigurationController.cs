using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Constants;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1;
using System.Net;

namespace OneApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.GlobalAdmin)]
public class ConfigurationController(IConfigurationService _configurationService) : ControllerBase
{
    [HttpGet("tenants")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllTenants()
    {
        var tenants = await _configurationService.GetAllTenants();
        return Ok(tenants);
    }

    [HttpGet("tenant/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetTenantById(string id)
    {
        var tenant = await _configurationService.GetTenantbyId(id);
        return tenant != null ? Ok(tenant) : NotFound();
    }

    [HttpPost("tenant")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request)
    {
        var tenantId = await _configurationService.CreateTenant(request);
        return CreatedAtAction(nameof(GetTenantById),new { id = tenantId }, null);
    }
}



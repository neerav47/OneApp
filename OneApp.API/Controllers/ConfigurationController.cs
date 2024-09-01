using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Interfaces;
using System.Net;

namespace OneApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public async Task<IActionResult> CreateTenant([FromBody] dynamic request)
    {
        await _configurationService.CreateTenant();
        return Created();
    }
}



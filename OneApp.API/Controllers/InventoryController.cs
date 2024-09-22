using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.Constants;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
using System.Net;

namespace OneApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InventoryController(IInventoryService _inventoryService) : ControllerBase
    {
        [HttpPut]
        [Authorize(Roles = Role.SystemAdmin)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateInventory([FromBody] UpdateInventoryRequest request)
        {
            var result = await _inventoryService.UpdateInventory(request);
            return Ok(result);
        }

    }
}

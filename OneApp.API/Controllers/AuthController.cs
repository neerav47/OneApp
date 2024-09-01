using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace OneApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<bool> Login([FromBody] object loginRequest)
    {
        await Task.Delay(1000);
        return true;
    }

    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<bool> Register([FromBody] object registerRequest)
    {
        await Task.Delay(1000);
        return true;
    }
}
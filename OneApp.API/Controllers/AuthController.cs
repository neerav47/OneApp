using System.Net;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1;

namespace OneApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : Controller
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse),(int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LogIn(request);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(TokenResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
    {
        var response = await _authService.RefreshToken(request);
        return Ok(response);
    }

    [HttpGet("v1/user")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetUser(string userName)
    {
        var result = await _authService.GetUser(userName);
        return result?.Count() > 0 ? Ok(result) : NotFound();
    }
}
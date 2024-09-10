using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1;
using System.Net;

namespace OneApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService _userService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest registerRequest)
    {
        var userId = await _userService.RegisterUser(registerRequest);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, null);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetUserById(id);
        return user != null ? Ok(user) : NotFound();
    }

    [HttpGet("all")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllUsers(string tenantId)
    {
        var users = await _userService.GetAllUsers(tenantId);
        return Ok(users);
    }

    [HttpPost("updateRoles")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateUserRoles([FromBody] UserRolesRequest request)
    {
        var isUpdated = await _userService.UpdateUserRoles(request);
        return Accepted(isUpdated);
    }

    [HttpGet("email")]
    [ProducesResponseType(typeof(UserDetailDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var userDetails = await _userService.GetUserByEmail(email);
        return userDetails != null ? Ok(userDetails) : NotFound();
    }
}

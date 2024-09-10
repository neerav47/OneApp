using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1;
using OneApp.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace OneApp.Business.Services;

public class AuthService(
    UserManager<User> _userManager,
    ILogger<AuthService> _logger,
    IPasswordHasher _passwordHasher,
    IConfiguration _configuration) : IAuthService
{
    #region Public methods
    public Task RefreshToken()
    {
        throw new NotImplementedException();
    }


    public async Task<string> LogIn(LoginRequest request)
    {
        _logger.LogInformation($"{nameof(LogIn)} started.");

        var user = await _userManager.FindByEmailAsync(request.UserName);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash ?? ""))
        {
            throw new Exception("Invalid username or password");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        return GenerateJwtToken(user, [.. userRoles]);
    }

    #endregion

    #region Private methods
    private string GenerateJwtToken(User user, string[] roles)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: "One",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        var tokenResponse = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenResponse;
        #endregion
    }
}

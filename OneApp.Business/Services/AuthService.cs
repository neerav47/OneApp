using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1;
using OneApp.Data.Context;
using OneApp.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OneApp.Business.Services;

public class AuthService(
    DataContext _context,
    ILogger<AuthService> _logger,
    IPasswordHasher _passwordHasher,
    IConfiguration _configuration,
    IUserService _userService,
    IMapper _mapper) : IAuthService
{
    private const short REFRESH_TOKEN_SIZE = 64;

    #region Public methods

    public async Task<TokenResponse> LogIn(LoginRequest request)
    {
        _logger.LogInformation($"{nameof(LogIn)} started.");

        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email!.ToLower().Equals(request.UserName.ToLower()));

        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash ?? ""))
        {
            throw new Exception("Invalid username or password");
        }

        var userDetails = await _userService.GetUserByEmail(user.Email!);

        return await GetTokenResponse(user, userDetails?.RoleNames ?? []);
    }

    public async Task<TokenResponse> RefreshToken(TokenRequest request)
    {
        _logger.LogInformation($"{nameof(RefreshToken)} started.");

        // Validate and extract userId
        var userId = ValidateJwt(request.AccessToken!);

        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.ToLower().Equals(userId.ToLower()));

        if (user == null ||
            user.RefreshToken == null ||
            user.RefreshTokenExpiry < DateTime.UtcNow ||
            !user.RefreshToken.Equals(request.RefreshToken))
        {
            _logger.LogInformation($"{nameof(RefreshToken)} invalid.");
            throw new Exception("Failed to validate refresh token.");
        }

        var userDetails = await _userService.GetUserByEmail(user.Email!);

        return await GetTokenResponse(user, userDetails?.RoleNames ?? []);
    }

    public async Task<IEnumerable<UserDto>> GetUser(string userName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));
        var result = await _context.Users
                           .Where(u => userName.ToLower().Equals(u.UserName!.ToLower()))
                           .Include(u => u.Tenant)
                           .AsSplitQuery()
                           .ToListAsync();
        return _mapper.Map<List<UserDto>>(result);
    }

    #endregion

    #region Private methods

    private (string, DateTime) GenerateJwtToken(User user, string[] roles)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new("tenant", user.TenantId.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenExpiry = DateTime.UtcNow.AddMinutes(30);
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: "One",
            claims: claims,
            expires: tokenExpiry,
            signingCredentials: credentials
        );

        var tokenResponse = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenResponse, tokenExpiry);
    }

    private async Task<(string, DateTime)> GenerateRefrestToken(User user)
    {
        string token = RandomNumberGenerator.GetHexString(REFRESH_TOKEN_SIZE);
        var tokenExpiry = DateTime.UtcNow.AddHours(12);
        await _userService.UpdateUserById(user.Id, new Dictionary<string, object>()
        {
            { nameof(user.RefreshToken), token },
            { nameof(user.RefreshTokenExpiry), tokenExpiry }
        });

        return (token, tokenExpiry);
    }

    private string ValidateJwt(string token)
    {
        _logger.LogInformation($"{nameof(ValidateJwt)} started.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = "One",
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

            // return user id from JWT token if validation successful
            return userId;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(ValidateJwt)} error validating token.");
            throw new Exception("Error validating token.", ex);
        }
    }

    private async Task<TokenResponse> GetTokenResponse(User user, string[] roles)
    {
        var response = new TokenResponse();
        (response.AccessToken, response.AccessTokenExpiration) = GenerateJwtToken(user, roles);
        (response.RefreshToken, response.RefreshTokenExpiration) = await GenerateRefrestToken(user);
        return response;
    }

    #endregion
}

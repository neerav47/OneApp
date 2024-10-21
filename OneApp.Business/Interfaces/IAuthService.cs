
using OneApp.Business.DTOs;
using OneApp.Contracts.v1;

namespace OneApp.Business.Interfaces;

public interface IAuthService
{
    Task<TokenResponse> LogIn(LoginRequest request);
    
    Task<TokenResponse> RefreshToken(TokenRequest request);

    Task<IEnumerable<UserDto>> GetUser(string userName);
}

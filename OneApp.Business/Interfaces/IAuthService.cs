
using OneApp.Business.DTOs;
using OneApp.Contracts.v1;

namespace OneApp.Business.Interfaces;

public interface IAuthService
{
    Task<string> LogIn(LoginRequest request);
    Task RefreshToken();
}

using OneApp.Contracts.v1;
using OneApp.Contracts.v1.Response;
using OneApp.Extentions;

namespace OneApp.Services.Interfaces;

public interface IAuthenticationService
{
    Task<TokenResponse> Login(LoginRequest request);

    Task<IEnumerable<User>> GetUser(string userName);

    Task SetUserContext(UserContext context);

    Task<UserContext> GetUserContext();
}

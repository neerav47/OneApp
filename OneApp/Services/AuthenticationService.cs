using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneApp.Contracts.v1;
using OneApp.Contracts.v1.Enums;
using OneApp.Contracts.v1.Response;
using OneApp.Extentions;
using OneApp.Services.Interfaces;
using System.Text;
namespace OneApp.Services;

internal class AuthenticationService(
    IDefaultHttpClient _httpClient,
    ILogger<AuthenticationService> _logger) : IAuthenticationService
{
    private const string USER_CONTEXT_KEY = "OneApp-UserContext";
    public async Task<IEnumerable<User>> GetUser(string userName)
    {
        _logger.LogInformation($"{nameof(GetUser)} started");
        var request = _httpClient.CreateHttpRequestMessage(new Uri($"{_httpClient.GetBaseAddress()}/api/Auth/v1/user?userName={userName}"), HttpMethod.Get);

        var response = await _httpClient.InvokeRequest(request);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<User>>(result);
        }

        return null;
    }

    public async Task<TokenResponse> Login(LoginRequest request)
    {
        _logger.LogInformation($"{nameof(Login)} started.");
        var requestMessage = _httpClient.CreateHttpRequestMessage(new Uri($"{_httpClient.GetBaseAddress()}/api/Auth/login"), HttpMethod.Post);

        var requestBody = JsonConvert.SerializeObject(request);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Content = content;

        var response = await _httpClient.InvokeRequest(requestMessage);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse>(result);
        }
        return null;
    }

    public async Task SetUserContext(UserContext context)
    {
        await SecureStorage.Default.SetAsync(USER_CONTEXT_KEY, JsonConvert.SerializeObject(context));
    }

    public async Task<UserContext> GetUserContext()
    {
        var value = await SecureStorage.Default.GetAsync(USER_CONTEXT_KEY);

        return !string.IsNullOrWhiteSpace(value) ? JsonConvert.DeserializeObject<UserContext>(value) : null;
    }

    public async Task<UserContext> RefreshUserContext()
    {
        _logger.LogInformation($"{nameof(AuthenticationService)}-{nameof(RefreshUserContext)} started.");
        // User context is not available, return
        var userContext = await GetUserContext();
        if (userContext is null)
        {
            return null;
        }
        // Refresh token expired or expiring in 1 minutes
        if (userContext.RefreshTokenExpiration < DateTime.UtcNow.AddMinutes(1))
        {
            return null;
        }
        // Http message
        var requestMessage = _httpClient.CreateHttpRequestMessage(new Uri($"{_httpClient.GetBaseAddress()}/api/Auth/refresh"), HttpMethod.Post);
        // Request body with access token and refresh token
        var requestBody = JsonConvert.SerializeObject(new TokenRequest
        {
            Type = TokenType.RefreshToken,
            AccessToken = userContext.AccessToken,
            AccessTokenExpiration = userContext.AccessTokenExpiration,
            RefreshToken = userContext.RefreshToken,
            RefreshTokenExpiration = userContext.RefreshTokenExpiration
        });
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Content = content;
        // Invoke request
        var response = await _httpClient.InvokeRequest(requestMessage);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);

            userContext.AccessToken = tokenResponse.AccessToken;
            userContext.RefreshToken = tokenResponse.RefreshToken;
            userContext.AccessTokenExpiration = tokenResponse.AccessTokenExpiration;
            userContext.RefreshTokenExpiration = tokenResponse.RefreshTokenExpiration;

            await SetUserContext(userContext);
        }
        _logger.LogInformation($"{nameof(AuthenticationService)}-{nameof(RefreshUserContext)} completed.");
        return userContext;
    }
}

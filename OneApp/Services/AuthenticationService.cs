using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneApp.Contracts.v1;
using OneApp.Contracts.v1.Response;
using OneApp.Extentions;
using OneApp.Services.Interfaces;
using System.Text;
using System.Text.Json.Serialization;
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

        if(response?.IsSuccessStatusCode == true)
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

    //public void SetUserContext(UserContext context)
    //{
    //    Preferences.Set(USER_CONTEXT_KEY, JsonConvert.SerializeObject(context));
    //}

    public async Task<UserContext> GetUserContext()
    {
        var value = await SecureStorage.Default.GetAsync(USER_CONTEXT_KEY);

        return !string.IsNullOrWhiteSpace(value) ? JsonConvert.DeserializeObject<UserContext>(value) : null;
    }

    //public UserContext GetUserContext()
    //{
    //    var value = Preferences.Default.Get<string>(USER_CONTEXT_KEY, null);

    //    return !string.IsNullOrWhiteSpace(value) ? JsonConvert.DeserializeObject<UserContext>(value) : null;
    //}
}

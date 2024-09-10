using Newtonsoft.Json;

namespace OneApp.Contracts.v1;

public class LoginRequest
{
    [JsonProperty("userName")]
    public string UserName { get; set; } = default!;

    [JsonProperty("password")]
    public string Password { get; set; } = default!;

    [JsonProperty("tenantId")]
    public string TenantId { get; set; } = default!;
}

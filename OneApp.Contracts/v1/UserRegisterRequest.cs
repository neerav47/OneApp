using Newtonsoft.Json;

namespace OneApp.Contracts.v1;

public class UserRegisterRequest
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; } = default!;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = default!;

    [JsonProperty("email")]
    public string Email { get; set; } = default!;

    [JsonProperty("password")]
    public string Password { get; set; } = default!;

    [JsonProperty("tenantId")]
    public Guid TenantId { get; set; }
}

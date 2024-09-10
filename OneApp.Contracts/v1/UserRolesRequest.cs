using Newtonsoft.Json;

namespace OneApp.Contracts.v1;

public class UserRolesRequest
{
    [JsonProperty("userId")]
    public string UserId { get; set; } = default!;

    [JsonProperty("tenantId")]
    public string TenantId { get; set; } = default!;

    [JsonProperty("rolesToAdd")]
    public List<string> RolesToAdd { get; set; } = default!;

    [JsonProperty("rolesToRemove")]
    public List<string> RolesToRemove { get; set; } = default!;
}

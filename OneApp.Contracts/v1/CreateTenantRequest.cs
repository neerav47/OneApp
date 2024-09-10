using Newtonsoft.Json;

namespace OneApp.Contracts.v1;

public class CreateTenantRequest
{
    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}

using Newtonsoft.Json;

namespace OneApp.Contracts.v1.Request;

public class CreateProductRequest
{
    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("description")]
    public string Description { get; set; } = default!;

    [JsonProperty("productTypeId")]
    public string ProductTypeId { get; set; } = default!;
}

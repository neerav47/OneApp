using Newtonsoft.Json;
using OneApp.Contracts.v1.Enums;

namespace OneApp.Contracts.v1.Request;

public class UpdateInventoryRequest
{
    [JsonProperty("productId")]
    public Guid ProductId { get; set; }

    [JsonProperty("inventoryUpdateType")]
    public InventoryUpdateType InventoryUpdateType { get; set; }

    [JsonProperty("value")]
    public int Value { get; set; }
}

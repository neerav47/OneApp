using OneApp.Contracts.v1.Request;
using OneApp.Data.Models;

namespace OneApp.Business.Interfaces;
public interface IInventoryService
{
    Task<bool> UpdateInventory(UpdateInventoryRequest request);

    Task AddInventoryHistory(Inventory inventory);
}

using OneApp.Contracts.v1.Request;

namespace OneApp.Business.Interfaces;
public interface IInventoryService
{
    Task<bool> UpdateInventory(UpdateInventoryRequest request);
}

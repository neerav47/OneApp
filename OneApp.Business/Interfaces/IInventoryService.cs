namespace OneApp.Business.Interfaces;
public interface IInventoryService
{
    Task CreateInventory();

    Task UpdateInventory();

    Task DeleteInventory();
}

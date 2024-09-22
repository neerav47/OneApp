using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Enums;
using OneApp.Contracts.v1.Request;
using OneApp.Data.Context;
using OneApp.Data.Models;
using OneApp.Data.Services;
using System.Data;

namespace OneApp.Business.Services;
public class InventoryService : IInventoryService
{
    private readonly DataContext _context;
    private readonly ITenantService _tenantService;
    private readonly ILogger<InventoryService> _logger;
    private readonly Guid _tenantId;
    private readonly Guid _userId;

    public InventoryService(
        DataContext dataContext,
        ITenantService tenantService,
        ILogger<InventoryService> logger)
    {
        this._context = dataContext;
        this._tenantService = tenantService;
        this._logger = logger;
        this._tenantId = (Guid)tenantService.GetTenantId();
        this._userId = (Guid)tenantService.GetUserId();
    }

    #region Public methods

    public async Task<bool> UpdateInventory(UpdateInventoryRequest request)
    {
        _logger.LogInformation($"{nameof(UpdateInventory)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(UpdateInventory)} transaction scope started.");
            // Add transaction
            var trans = await _context.Transaction.AddAsync(new Transaction
            {
                CreatedBy = _userId,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedBy = _userId,
                LastUpdatedDate = DateTime.UtcNow,
            });
            await _context.SaveChangesAsync();
            // Get inventory
            var inventory = await _context.Inventory.SingleAsync(i => i.ProductId == request.ProductId);
            // Add inventory history
            await AddInventoryHistory(inventory);
            // Update inventory
            var inventoryQuantity = request.Value * (request.InventoryUpdateType == InventoryUpdateType.Add ? 1 : -1);
            await _context.Inventory.Where(i => i.ProductId == request.ProductId)
                                    .ExecuteUpdateAsync(i => i.SetProperty(p => p.Quantity, p => p.Quantity + inventoryQuantity)
                                                              .SetProperty(p => p.LastUpdatedBy, _userId)
                                                              .SetProperty(p => p.LastUpdatedDate, DateTime.UtcNow)
                                                              .SetProperty(p => p.TransactionId, trans.Entity.Id));
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation($"{nameof(UpdateInventory)} transaction scope completed.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Failed to update inventory.");
            throw new Exception("Failed to update inventory.", ex);
        }
        return true;
    }

    #endregion

    #region Private methods
    private async Task AddInventoryHistory(Inventory inventory)
    {
        await _context.InventoryHistory.AddAsync(new InventoryHistory
        {
            Id = Guid.NewGuid(),
            InvetoryId = inventory.Id,
            ProductId = inventory.ProductId,
            Quantity = inventory.Quantity,
            TransactionId = inventory.TransactionId,
            CreatedBy = inventory.CreatedBy,
            CreatedDate = inventory.CreatedDate,
            LastUpdatedBy = inventory.LastUpdatedBy,
            LastUpdatedDate = inventory.LastUpdatedDate,
            TenantId = inventory.TenantId
        });
    }
    #endregion
}

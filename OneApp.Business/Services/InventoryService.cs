﻿using OneApp.Business.Interfaces;
using OneApp.Data.Context;

namespace OneApp.Business.Services;
public class InventoryService(DataContext _context) : IInventoryService
{
    #region Public methods
    public Task CreateInventory()
    {
        throw new NotImplementedException();
    }

    public Task DeleteInventory()
    {
        throw new NotImplementedException();
    }

    public Task UpdateInventory()
    {
        throw new NotImplementedException();
    }

    #endregion
}

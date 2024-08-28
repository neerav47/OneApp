using Microsoft.EntityFrameworkCore;
using OneApp.Data.Context;
using OneApp.Data.Interfaces;
using OneApp.Data.Models;

namespace OneApp.Data.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly DataContext _dataContext;

    public ConfigurationService(DataContext dataContext)
    {
        this._dataContext = dataContext;
    }
    public Task CreateTenant()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Tenant>> GetAllTenants()
    {
        return await _dataContext.Tenant.ToListAsync();
    }

    public async Task<Tenant?> GetTenantbyId(string id)
    {
        var guidId = Guid.Parse(id);
        return await _dataContext.Tenant.SingleOrDefaultAsync(t => t.Id.Equals(guidId));
    }
}

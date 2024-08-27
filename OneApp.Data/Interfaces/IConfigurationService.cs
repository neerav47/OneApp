using OneApp.Data.Models;

namespace OneApp.Data.Interfaces;

public interface IConfigurationService
{
    public Task CreateTenant();

    public Task<IEnumerable<Tenant>> GetAllTenants();

    public Task<Tenant?> GetTenantbyId(string id);
}

using OneApp.Business.DTOs;
namespace OneApp.Business.Interfaces;

public interface IConfigurationService
{
    public Task CreateTenant();

    public Task<IEnumerable<TenantDto>> GetAllTenants();

    public Task<TenantDto?> GetTenantbyId(string id);
}

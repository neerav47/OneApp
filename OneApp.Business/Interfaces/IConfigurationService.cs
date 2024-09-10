using OneApp.Business.DTOs;
using OneApp.Contracts.v1;
namespace OneApp.Business.Interfaces;

public interface IConfigurationService
{
    public Task<Guid> CreateTenant(CreateTenantRequest request);
    public Task<IEnumerable<TenantDto>> GetAllTenants();
    public Task<TenantDto?> GetTenantbyId(string id);
    public Task<bool> IsTenantNameUnique(string name);
}

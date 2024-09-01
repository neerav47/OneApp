using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Data.Context;

namespace OneApp.Business.Services;

public class ConfigurationService(DataContext _context, IMapper _mapper) : IConfigurationService
{
    public Task CreateTenant()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TenantDto>> GetAllTenants()
    {
        var tenants = await _context.Tenant.ToListAsync();
        return _mapper.Map<List<TenantDto>>(tenants);
    }

    public async Task<TenantDto?> GetTenantbyId(string id)
    {
        var tenant = await _context.Tenant.SingleOrDefaultAsync(t => t.Id.Equals(Guid.Parse(id)));
        return _mapper.Map<TenantDto?>(tenant);
    }
}

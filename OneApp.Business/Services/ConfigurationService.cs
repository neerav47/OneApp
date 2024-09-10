using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1;
using OneApp.Data.Context;
using OneApp.Data.Models;
using System.Data;

namespace OneApp.Business.Services;

public class ConfigurationService(
    DataContext _context,
    IMapper _mapper,
    ILogger<ConfigurationService> _logger) : IConfigurationService
{
    #region Public methods
    public async Task<Guid> CreateTenant(CreateTenantRequest request)
    {
        _logger.LogInformation("Create tenant started.");

        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(CreateTenant)} transaction scope started.");
            var tenant = new Tenant { Id = Guid.NewGuid(), Name = request.Name };
            await _context.Tenant.AddAsync(tenant);
            await _context.SaveChangesAsync();
            transaction.Commit();
            _logger.LogInformation($"{nameof(CreateTenant)} transaction scope completed.");
            return tenant.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create tenant");
            transaction.Rollback();
            throw new Exception("Failed to create tenant");
        }
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

    public async Task<bool> IsTenantNameUnique(string name)
    {
        return !await _context.Tenant.AnyAsync(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    #endregion
}

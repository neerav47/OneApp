using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
using OneApp.Data.Context;
using OneApp.Data.Models;
using OneApp.Data.Services;
using System.Data;

namespace OneApp.Business.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly DataContext _context;
    private readonly ITenantService _tenantService;
    private readonly IMapper _mapper;
    private readonly Guid _tenantId;
    private readonly Guid _userId;

    public CustomerService(
        DataContext context,
        ITenantService tenantService,
        ILogger<CustomerService> logger,
        IMapper mapper)
    {
        this._context = context;
        this._tenantService = tenantService;
        this._logger = logger;
        this._mapper = mapper;
        this._tenantId = (Guid)tenantService.GetTenantId()!;
        this._userId = (Guid)tenantService.GetUserId()!;
    }
    public async Task<Guid> AddCustomer(CreateCustomerRequest request)
    {
        _logger.LogInformation($"{nameof(AddCustomer)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(AddCustomer)} transaction scope started.");
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                CreatedBy = _userId,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedBy = _userId,
                LastUpdatedDate = DateTime.UtcNow,
                TenantId = _tenantId,
            };

            await _context.Customer.AddAsync(customer);
            await _context.SaveChangesAsync();
            transaction.Commit();
            _logger.LogInformation($"{nameof(AddCustomer)} transaction scope completed.");
            return customerId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create customer");
            transaction.Rollback();
            throw new Exception("Failed to create customer");
        }
    }

    public async Task<CustomerDto?> GetCustomerbyId(string id)
    {
        var customer = await _context.Customer.SingleOrDefaultAsync(c => c.Id == Guid.Parse(id) && c.TenantId == _tenantId);
        return _mapper.Map<CustomerDto?>(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomers(){
        var customers = await _context.Customer.Where(c => c.TenantId == _tenantId).ToListAsync();
        return _mapper.Map<List<CustomerDto>>(customers);
    }
}

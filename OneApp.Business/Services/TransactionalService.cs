using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
using OneApp.Data.Context;
using OneApp.Data.Enums;
using OneApp.Data.Models;
using OneApp.Data.Services;
using System.Data;

namespace OneApp.Business.Services;

public sealed class TransactionalService : ITransactionalService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly DataContext _context;
    private readonly ITenantService _tenantService;
    private readonly IMapper _mapper;
    private readonly Guid _tenantId;
    private readonly Guid _userId;

    public TransactionalService(
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

    public async Task<Guid> CreateInvoice(CreateInvoiceRequest request)
    {
        _logger.LogInformation($"{nameof(CreateInvoice)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(CreateInvoice)} transaction scope started.");
            var receiptId = Guid.NewGuid();
            var receipt = new TReceipt
            {
                Id = receiptId,
                CustomerId = request.CustomerId,
                Status = Status.Created,
                CreatedBy = _userId,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedBy = _userId,
                LastUpdatedDate = DateTime.UtcNow,
                TenantId = _tenantId,
            };

            await _context.TReceipt.AddAsync(receipt);
            await _context.SaveChangesAsync();
            transaction.Commit();
            _logger.LogInformation($"{nameof(CreateInvoice)} transaction scope completed.");
            return receiptId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create invoice");
            transaction.Rollback();
            throw new Exception("Failed to create invoice");
        }
    }

    public async Task<InvoiceDto?> GetInvoiceById(string id)
    {
        var invoice = await _context.TReceipt
                                    .Include(t => t.Customer)
                                    .Include(t => t.SaleItems)
                                    .AsSplitQuery()
                                    .SingleOrDefaultAsync(t => t.Id == Guid.Parse(id) && 
                                                          t.TenantId == _tenantId &&
                                                          !t.IsDeleted);
        return _mapper.Map<InvoiceDto?>(invoice);
    }

    public Task AddInvoiceItem()
    {
        throw new NotImplementedException();
    }

    public Task CheckOut()
    {
        throw new NotImplementedException();
    }

    public Task DeleteInvoice(string id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteInvoiceItem()
    {
        throw new NotImplementedException();
    }

    public Task EditInvoiceItem()
    {
        throw new NotImplementedException();
    }
}

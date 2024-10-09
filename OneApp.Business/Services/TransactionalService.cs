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

    #region TReceipt

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

    public async Task<bool> DeleteInvoice(string id)
    {
        var result = false;
        _logger.LogInformation($"{nameof(DeleteInvoice)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(DeleteInvoice)} transaction scope started.");
            var count = await _context.TReceipt
                                      .Where(t => t.TenantId == this._tenantId && t.Id == Guid.Parse(id))
                                      .ExecuteUpdateAsync(p => p.SetProperty(pt => pt.IsDeleted, true)
                                                                .SetProperty(pt => pt.LastUpdatedBy, _userId)
                                                                .SetProperty(pt => pt.LastUpdatedDate, DateTime.UtcNow));
            await transaction.CommitAsync();
            result = count > 0;
            _logger.LogInformation($"{nameof(DeleteInvoice)} transaction scope completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteInvoice)} failed.");
            throw new Exception("Failed to delete product", ex);
        }
        return result;
    }

    #endregion

    #region TSaleItem

    public async Task<Guid> AddInvoiceItem(AddInvoiceItemRequest request)
    {
        _logger.LogInformation($"{nameof(AddInvoiceItem)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(AddInvoiceItem)} transaction scope started.");
            var saleItemId = Guid.NewGuid();
            var saleItem = new TSaleItem
            {
                Id = saleItemId,
                ReceiptId = request.ReceiptId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = (int)request.UnitPrice,
                CreatedBy = _userId,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedBy = _userId,
                LastUpdatedDate = DateTime.UtcNow,
                TenantId = _tenantId,
            };

            var saleItemEntity = await _context.TSaleItem.AddAsync(saleItem);
            await _context.SaveChangesAsync();
            transaction.Commit();
            _logger.LogInformation($"{nameof(AddInvoiceItem)} transaction scope completed.");
            return saleItemEntity.Entity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create invoice");
            transaction.Rollback();
            throw new Exception("Failed to create invoice");
        }
    }

    public async Task<bool> DeleteInvoiceItem(Guid invoiceId, Guid invoiceItemId)
    {
        var result = false;
        _logger.LogInformation($"{nameof(DeleteInvoiceItem)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(DeleteInvoiceItem)} transaction scope started.");
            var count = await _context.TSaleItem
                                      .Where(t => t.TenantId == this._tenantId && 
                                                  t.Id == invoiceItemId &&
                                                  t.ReceiptId == invoiceId &&
                                                  !t.IsDeleted)
                                      .ExecuteUpdateAsync(p => p.SetProperty(pt => pt.IsDeleted, true)
                                                                .SetProperty(pt => pt.LastUpdatedBy, _userId)
                                                                .SetProperty(pt => pt.LastUpdatedDate, DateTime.UtcNow));
            await transaction.CommitAsync();
            result = count > 0;
            _logger.LogInformation($"{nameof(DeleteInvoiceItem)} transaction scope completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteInvoiceItem)} failed.");
            throw new Exception("Failed to delete invoice item", ex);
        }
        return result;
    }

    public async Task<bool> EditInvoiceItem(Guid invoiceId, Guid invoiceItemId, EditInvoiceItemRequest request)
    {
        var result = false;
        _logger.LogInformation($"{nameof(EditInvoiceItem)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(EditInvoiceItem)} transaction scope started.");
            var count = await _context.TSaleItem
                          .Where(s => !s.IsDeleted && 
                                       s.ReceiptId == invoiceId && 
                                       s.Id == invoiceItemId && 
                                       s.TenantId == this._tenantId)
                          .ExecuteUpdateAsync(s => s.SetProperty(s => s.UnitPrice, request.UnitPrice)
                                                    .SetProperty(s => s.Quantity, request.Quantity)
                                                    .SetProperty(s => s.LastUpdatedBy, _userId)
                                                    .SetProperty(s => s.LastUpdatedDate, DateTime.UtcNow));
            result = count > 0;
            await transaction.CommitAsync();
            _logger.LogInformation($"{nameof(EditInvoiceItem)} transaction scope completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(EditInvoiceItem)} failed.");
            throw new Exception("Failed to invoice item", ex);
        }
        return result;
    }

    public async Task<IEnumerable<InvoiceItemDto>> GetInvoiceItemsByInvoiceId(string invoiceId)
    {
        var saleItems = await _context.TSaleItem.Where(s => !s.IsDeleted &&
                                                             s.ReceiptId == Guid.Parse(invoiceId) &&
                                                             s.TenantId == this._tenantId).ToListAsync();

        return _mapper.Map<List<InvoiceItemDto>>(saleItems);
    }

    #endregion

    public Task CheckOut()
    {
        throw new NotImplementedException();
    }
}

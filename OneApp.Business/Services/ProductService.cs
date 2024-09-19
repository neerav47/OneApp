using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OneApp.Business.Constants;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1.Request;
using OneApp.Data.Context;
using OneApp.Data.Models;
using OneApp.Data.Services;
using System.Data;

namespace OneApp.Business.Services;

public class ProductService: IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly ITenantService _tenantService;

    private readonly Guid _tenantId;
    private readonly Guid _userId;
    public ProductService(DataContext context,
                        IMapper mapper,
                        ILogger<ProductService> logger,
                        ITenantService tenantService)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = logger;
        this._tenantService = tenantService;
        this._tenantId = (Guid)tenantService.GetTenantId()!;
        this._userId = (Guid)tenantService.GetUserId()!;
    }

    #region Public methods

    #region Product
    public async Task<ProductDto> CreateProduct(CreateProductRequest request)
    {
        _logger.LogInformation($"{nameof(CreateProduct)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        ProductDto productDto;
        try
        {
            _logger.LogInformation($"{nameof(CreateProduct)} transaction scope started.");
            // Add transaction
            var trans = await _context.Transaction.AddAsync(AddTransaction(context));
            await _context.SaveChangesAsync();
            // Add product
            var product = await _context.Product.AddAsync(AddProduct(request, context));
            // Add inventory
            var inventory = await _context.Inventory.AddAsync(AddInventory(product.Entity, trans.Entity, context));
            await _context.SaveChangesAsync();
            productDto = _mapper.Map<ProductDto>(product.Entity);
            await transaction.CommitAsync();
            _logger.LogInformation($"{nameof(CreateProduct)} transaction scope completed.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Failed to create product.");
            throw new Exception("Failed to create product.", ex);
        }

        return productDto;
    }

    public async Task<bool> DeleteProductById(string id)
    {
        var result = false;
        _logger.LogInformation($"{nameof(DeleteProductById)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(DeleteProductById)} transaction scope started.");
            var count = await _context.Product
                          .Where(p => p.Id == Guid.Parse(id) && p.TenantId == this._tenantId)
                          .ExecuteUpdateAsync(p => p.SetProperty(pt => pt.IsDeleted, true)
                                                    .SetProperty(pt => pt.LastUpdatedBy, _userId)
                                                    .SetProperty(pt => pt.LastUpdatedDate, DateTime.UtcNow));
            result = count > 0;
            await transaction.CommitAsync();
            _logger.LogInformation($"{nameof(DeleteProductById)} transaction scope completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteProductById)} failed.");
            throw new Exception("Failed to delete product", ex);
        }
        return result;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var products = await _context.Product
                                     .Include(p => p.ProductType)
                                     .Include(p => p.Inventory)
                                     .AsSplitQuery()
                                     .Where(p => !p.IsDeleted)
                                     .ToListAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductById(string id)
    {
        var product = await _context.Product
                                    .Include(p => p.ProductType)
                                    .Include(p => p.Inventory)
                                    .AsSplitQuery()
                                    .SingleOrDefaultAsync(p => p.Id == Guid.Parse(id) && !p.IsDeleted);
        return _mapper.Map<ProductDto?>(product);
    }

    #endregion

    #region ProductType

    public async Task<ProductTypeDto> CreateProductType(CreateProductTypeRequest request)
    {
        _logger.LogInformation($"{nameof(CreateProductType)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        EntityEntry<ProductType> entity;
        try
        {
            _logger.LogInformation($"{nameof(CreateProductType)} transaction scope started.");
            var productType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                TenantId = _tenantId,
                CreatedBy = _userId,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedBy = _userId,
                LastUpdatedDate = DateTime.UtcNow
            };
            entity = await _context.ProductType.AddAsync(productType);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation($"{nameof(CreateProductType)} transaction scope completed.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Failed to create product type.");
            throw new Exception("Failed to create product type.", ex);
        }
        return _mapper.Map<ProductTypeDto>(entity.Entity);
    }

    public async Task<IEnumerable<ProductTypeDto>> GetAllProductTypes()
    {
        var productTypes = await _context.ProductType.ToListAsync();
        return _mapper.Map<List<ProductTypeDto>>(productTypes);
    }

    public async Task<ProductTypeDto?> GetProductTypeById(string id)
    {
        var productType = await _context.ProductType.SingleOrDefaultAsync(p => p.Id == Guid.Parse(id));
        return _mapper.Map<ProductTypeDto?>(productType);
    }

    #endregion

    #endregion

    #region Private methods
    private Product AddProduct(CreateProductRequest request)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            ProductTypeId = Guid.Parse(request.ProductTypeId),
            CreatedBy = _userId,
            CreatedDate = DateTime.UtcNow,
            LastUpdatedBy = _userId,
            LastUpdatedDate = DateTime.UtcNow,
            TenantId = _tenantId
        };
    }

    private Inventory AddInventory(Product product, Transaction transaction)
    {
        return new Inventory
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            Quantity = 0,
            TransactionId = transaction.Id,
            TenantId = _tenantId,
            CreatedBy = _userId,
            CreatedDate = DateTime.UtcNow,
            LastUpdatedBy = _userId,
            LastUpdatedDate = DateTime.UtcNow
        };
    }

    private Transaction AddTransaction()
    {
        return new Transaction
        {
            CreatedBy = _userId,
            CreatedDate = DateTime.UtcNow,
            LastUpdatedBy = _userId,
            LastUpdatedDate = DateTime.UtcNow
        };
    }

    #endregion 
}



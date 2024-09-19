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
using System.Data;

namespace OneApp.Business.Services;

public class ProductService(
    DataContext _context,
    IMapper _mapper,
    ILogger<ProductService> _logger) : IProductService
{
    #region Public methods

    #region Product
    public async Task<ProductDto> CreateProduct(CreateProductRequest request, Context context)
    {
        _logger.LogInformation($"{nameof(CreateProduct)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        ProductDto productDto;
        try
        {
            _logger.LogInformation($"{nameof(CreateProduct)} transaction scope started.");
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                ProductTypeId = Guid.Parse(request.ProductTypeId),
                CreatedBy = context.UserId,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedBy = context.UserId,
                LastUpdatedDate = DateTime.UtcNow,
                TenantId = context.TenantId
            };
            var entity = await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
            productDto = _mapper.Map<ProductDto>(entity.Entity);
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

    public async Task<bool> DeleteProductById(string id, Context context)
    {
        _logger.LogInformation($"{nameof(DeleteProductById)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(DeleteProductById)} transaction scope started.");
            await _context.Product.ExecuteUpdateAsync(p => p.SetProperty(pt => pt.IsDeleted, true));
            await transaction.CommitAsync();
            _logger.LogInformation($"{nameof(DeleteProductById)} transaction scope completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteProductById)} failed.");
            throw new Exception("Failed to delete product", ex);
        }
        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts(Context context)
    {
        var products = await _context.Product.Include(p => p.ProductType).Where(p => p.TenantId == context.TenantId && !p.IsDeleted).ToListAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductById(string id)
    {
        var product = await _context.Product.Include(p => p.ProductType).SingleOrDefaultAsync(p => p.Id == Guid.Parse(id) && !p.IsDeleted);
        return _mapper.Map<ProductDto?>(product);
    }

    #endregion

    #region ProductType

    public async Task<ProductTypeDto> CreateProductType(CreateProductTypeRequest request, Context context)
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
                TenantId = context.TenantId,
                CreatedBy = context.UserId,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedBy = context.UserId,
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

    public async Task<IEnumerable<ProductTypeDto>> GetAllProductTypes(Context context)
    {
        var productTypes = await _context.ProductType.Where(p => p.TenantId == context.TenantId).ToListAsync();
        return _mapper.Map<List<ProductTypeDto>>(productTypes);
    }

    public async Task<ProductTypeDto?> GetProductTypeById(string id)
    {
        var productType = await _context.ProductType.SingleOrDefaultAsync(p => p.Id == Guid.Parse(id));
        return _mapper.Map<ProductTypeDto?>(productType);
    }

    #endregion

    #endregion
}



using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

    public Task CreateProduct()
    {
        throw new NotImplementedException();
    }

    public async Task<ProductTypeDto> CreateProductType(CreateProductTypeRequest request, Context context)
    {
        _logger.LogInformation($"{nameof(CreateProductType)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
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
            var entity = await _context.ProductType.AddAsync(productType);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation($"{nameof(CreateProductType)} transaction scope completed.");
            return _mapper.Map<ProductTypeDto>(entity.Entity);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Failed to create product type.");
            throw new Exception("Failed to create product type.", ex);
        }
    }

    public Task DeleteProductById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var products = await _context.Product.ToListAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductTypeDto>> GetAllProductTypes()
    {
        var productTypes = await _context.ProductType.ToListAsync();
        return _mapper.Map<List<ProductTypeDto>>(productTypes);
    }

    public async Task<ProductDto?> GetProductById(string id)
    {
        var product = await _context.Product.Where(p => p.Id.Equals(Guid.Parse(id))).FirstOrDefaultAsync();
        return _mapper.Map<ProductDto?>(product);
    }

    public async Task<ProductTypeDto?> GetProductTypeById(string id)
    {
        var productType = await _context.ProductType.SingleOrDefaultAsync(p => p.Id == Guid.Parse(id));
        return _mapper.Map<ProductTypeDto?>(productType);
    }

    #endregion
}



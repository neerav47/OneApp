using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Data.Context;

namespace OneApp.Business.Services;

public class ProductService(DataContext _context, IMapper _mapper) : IProductService
{
    #region Public methods

    public Task CreateProduct()
    {
        throw new NotImplementedException();
    }

    public Task CreateProductType()
    {
        throw new NotImplementedException();
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

    #endregion
}



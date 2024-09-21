using OneApp.Business.Constants;
using OneApp.Business.DTOs;
using OneApp.Contracts.v1.Request;

namespace OneApp.Business.Interfaces;
public interface IProductService
{
    Task<ProductDto> CreateProduct(CreateProductRequest request);

    Task<ProductDto?> GetProductById(string id);

    Task<IEnumerable<ProductDto>> GetAllProducts();

    Task<bool> DeleteProductById(string id);

    Task<IEnumerable<ProductTypeDto>> GetAllProductTypes();

    Task<ProductTypeDto> CreateProductType(CreateProductTypeRequest request);

    Task<ProductTypeDto?> GetProductTypeById(string id);
}

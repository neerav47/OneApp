using OneApp.Business.DTOs;

namespace OneApp.Business.Interfaces;
public interface IProductService
{
    Task CreateProduct();

    Task<ProductDto?> GetProductById(string id);

    Task<IEnumerable<ProductDto>> GetAllProducts();

    Task DeleteProductById(string id);

    Task<IEnumerable<ProductTypeDto>> GetAllProductTypes();

    Task CreateProductType();
}

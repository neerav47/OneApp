using OneApp.Business.Constants;
using OneApp.Business.DTOs;
using OneApp.Contracts.v1.Request;

namespace OneApp.Business.Interfaces;
public interface IProductService
{
    Task CreateProduct();

    Task<ProductDto?> GetProductById(string id);

    Task<IEnumerable<ProductDto>> GetAllProducts();

    Task DeleteProductById(string id);

    Task<IEnumerable<ProductTypeDto>> GetAllProductTypes();

    Task<ProductTypeDto> CreateProductType(CreateProductTypeRequest request, Context context);

    Task<ProductTypeDto?> GetProductTypeById(string id);
}

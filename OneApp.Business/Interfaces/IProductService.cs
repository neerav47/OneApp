using OneApp.Business.Constants;
using OneApp.Business.DTOs;
using OneApp.Contracts.v1.Request;

namespace OneApp.Business.Interfaces;
public interface IProductService
{
    Task<ProductDto> CreateProduct(CreateProductRequest request, Context context);

    Task<ProductDto?> GetProductById(string id);

    Task<IEnumerable<ProductDto>> GetAllProducts(Context context);

    Task<bool> DeleteProductById(string id, Context context);

    Task<IEnumerable<ProductTypeDto>> GetAllProductTypes(Context context);

    Task<ProductTypeDto> CreateProductType(CreateProductTypeRequest request, Context context);

    Task<ProductTypeDto?> GetProductTypeById(string id);
}

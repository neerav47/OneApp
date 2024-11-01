using System;
using OneApp.Contracts.v1.Request;
using OneApp.Contracts.v1.Response;

namespace OneApp.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProducts();

    Task<bool> UpdateInventory(UpdateInventoryRequest request);

    Task<IEnumerable<ProductType>> GetProductTypes();

    Task<bool> CreateProduct(CreateProductRequest request);
}


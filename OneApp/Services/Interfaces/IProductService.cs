using System;
using OneApp.Contracts.v1.Response;

namespace OneApp.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProducts();
}


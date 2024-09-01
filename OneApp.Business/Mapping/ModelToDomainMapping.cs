using AutoMapper;
using OneApp.Business.DTOs;
using OneApp.Data.Models;

namespace OneApp.Business.Mapping;

public class ModelToDomainMapping : Profile
{
    public ModelToDomainMapping()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Inventory, InventoryDto>();
        CreateMap<Tenant, TenantDto>();
    }
}

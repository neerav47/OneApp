using AutoMapper;
using OneApp.Business.DTOs;
using OneApp.Contracts.v1.Response;
using OneApp.Data.Models;

namespace OneApp.Business.Mapping;

public class ModelToDomainMapping : Profile
{
    public ModelToDomainMapping()
    {
        CreateMap<Data.Models.Product, ProductDto>();
        CreateMap<Data.Models.Inventory, InventoryDto>();
        CreateMap<Data.Models.Tenant, TenantDto>();
        CreateMap<Data.Models.ProductType, ProductTypeDto>();
        CreateMap<Data.Models.User, UserDto>();
        CreateMap<Data.Models.Customer, CustomerDto>();
        CreateMap<TReceipt, InvoiceDto>().ForMember(dest => dest.InvoiceItems, opt => opt.MapFrom(src => src.SaleItems));
        CreateMap<TSaleItem, InvoiceItemDto>();

        CreateMap<InvoiceDto, Invoice>();
        CreateMap<InvoiceItemDto, InvoiceItem>();
        CreateMap<CustomerDto, Contracts.v1.Response.Customer>();
        CreateMap<ProductDto, Contracts.v1.Response.Product>();
        CreateMap<ProductTypeDto, Contracts.v1.Response.ProductType>();
        CreateMap<InventoryDto, Contracts.v1.Response.Inventory>();
    }
}

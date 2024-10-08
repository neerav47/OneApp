﻿using AutoMapper;
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
        CreateMap<ProductType, ProductTypeDto>();
        CreateMap<User, UserDto>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<TReceipt, InvoiceDto>().ForMember(dest => dest.InvoiceItems, opt => opt.MapFrom(src => src.SaleItems));
        CreateMap<TSaleItem, InvoiceItemDto>();
    }
}
